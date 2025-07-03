using MediatR;
using SmartStockAI.Application.DTOs.Notifications;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Application.Interfaces.Notifications;
using SmartStockAI.Application.UsesCases.Notifications.Commands;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Infrastructure.Notifications.Services;

public class AlertaSistemaService : IAlertaSistemaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public AlertaSistemaService(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task VerificarProductosConStockBajoAsync()
    {
        var negocios = await _unitOfWork.NegociosRepository.ObtenerTodosAsync();

        foreach (var negocio in negocios)
        {
            var productos = await _unitOfWork.ProductosRepository.GetAllByNegocioAsync(negocio.Id);

            foreach (var producto in productos)
            {
                if (producto.Stock <= producto.Umbral)
                {
                    var yaNotificado = await _unitOfWork.NotificacionRepository
                        .ExisteNotificacionAsync(negocio.Id, producto.Id, "Stock bajo");

                    if (yaNotificado) continue;

                    var dto = new RegistrarNotificacionDto
                    {
                        Titulo = "Stock bajo",
                        Mensaje = $"El producto {producto.Nombre} está por debajo del umbral ({producto.Stock} unidades restantes).",
                        IdProducto = producto.Id,
                        IdNegocio = producto.IdNegocio
                    };

                    await _mediator.Send(new RegistrarNotificacionCommand(dto));
                }
            }
        }
    }

    public async Task VerificarProductosConBajaRotacionAsync()
    {
        var negocios = await _unitOfWork.NegociosRepository.ObtenerTodosAsync();

        foreach (var negocio in negocios)
        {
            var productos = await _unitOfWork.ProductosRepository.GetAllByNegocioAsync(negocio.Id);

            foreach (var producto in productos)
            {
                var huboVentas = await _unitOfWork.SaleRepository
                    .HayVentasRecientesAsync(producto.Id, negocio.Id, DateTime.UtcNow.AddDays(-2));

                if (!huboVentas)
                {
                    var yaNotificado = await _unitOfWork.NotificacionRepository
                        .ExisteNotificacionAsync(negocio.Id, producto.Id, "Baja rotación");

                    if (yaNotificado) continue;

                    var dto = new RegistrarNotificacionDto
                    {
                        Titulo = "Baja rotación",
                        Mensaje = $"El producto {producto.Nombre} no se ha vendido en los últimos 2 dias.",
                        IdProducto = producto.Id,
                        IdNegocio = producto.IdNegocio
                    };

                    await _mediator.Send(new RegistrarNotificacionCommand(dto));
                }
            }
        }
    }

    public async Task GenerarAlertaPorVentaAsync(int idVenta)
    {
        var venta = await _unitOfWork.SaleRepository.GetByIdAsync(idVenta);
        if (venta == null) return;
        
        var detalles = await _unitOfWork.DetailSaleRepository.GetByVentaIdAsync(idVenta);
        if (detalles == null || detalles.Count() == 0) return;
        
        int totalProductos = detalles.Sum(d => d.Cantidad);
        
        var dto = new RegistrarNotificacionDto
        {
            Titulo = "Venta registrada",
            Mensaje = $"Se registró una venta de {totalProductos} producto(s) por un total de S/ {venta.TotalVenta:F2}.",
            IdVenta = venta.Id,
            IdNegocio = venta.IdNegocio
        };

        await _mediator.Send(new RegistrarNotificacionCommand(dto));
    }

    public async Task GenerarAlertaPorMovimientoAsync(int idMovimiento)
    {
        var movimiento = await _unitOfWork.MovimientoInventarioRepository.GetByIdAsync(idMovimiento);
        if (movimiento == null) return;

        var tipo = movimiento.TipoMovimiento;

        var dto = new RegistrarNotificacionDto
        {
            Titulo = $"Movimiento de {tipo}",
            Mensaje = $"Se registró un movimiento de tipo {tipo} de {movimiento.Cantidad} unidades del producto {movimiento.Producto.Nombre}.",
            IdMovimiento = movimiento.Id,
            IdNegocio = movimiento.IdNegocio
        };
        await _mediator.Send(new RegistrarNotificacionCommand(dto));
    }
}