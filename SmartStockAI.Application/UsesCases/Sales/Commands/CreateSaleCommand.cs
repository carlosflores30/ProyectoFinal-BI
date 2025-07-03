using AutoMapper;
using MediatR;
using System;
using Hangfire;
using SmartStockAI.Application.DTOs.Sales;
using SmartStockAI.Application.Interfaces.Notifications;
using SmartStockAI.Domain.Inventory.Entities;
using SmartStockAI.Domain.Sales.Entities;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Sales.Commands;

public record CreateSaleCommand(CreateSaleDto SaleDto, int IdNegocio, int IdUsuario) : IRequest<int>;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSaleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        decimal totalVenta = 0;
        var detallesVenta = new List<DetalleDeVenta>();
        var movimientos = new List<MovimientoInventario>();

        foreach (var item in request.SaleDto.Detalles)
        {
            var producto = await _unitOfWork.ProductosRepository.GetByIdAsync(item.IdProducto);

            if (producto == null || producto.IdNegocio != request.IdNegocio)
                throw new ApplicationException($"Producto {item.IdProducto} no válido.");

            if (producto.Stock < item.Cantidad)
                throw new ApplicationException($"Stock insuficiente para el producto: {producto.Nombre}");

            var precioUnitario = producto.PrecioVenta;

            if (producto.PrecioDescuento is decimal descuento && descuento < producto.PrecioVenta)
            {
                precioUnitario = descuento;
            }

            var subtotal = (precioUnitario - item.DescuentoAplicado) * item.Cantidad;
            totalVenta += subtotal;

            // Agregar detalle
            detallesVenta.Add(new DetalleDeVenta
            {
                IdProducto = item.IdProducto,
                Cantidad = item.Cantidad,
                PrecioUnitario = precioUnitario,
                DescuentoAplicado = item.DescuentoAplicado
            });

            // Actualizar stock
            producto.Stock -= item.Cantidad;
            _unitOfWork.ProductosRepository.Update(producto);

            // Movimiento de inventario
            movimientos.Add(new MovimientoInventario
            {
                IdProducto = item.IdProducto,
                TipoMovimiento = "Venta",
                Cantidad = item.Cantidad,
                FechaMovimiento = DateTime.UtcNow,
                Observacion = $"Venta registrada",
                IdUsuario = request.IdUsuario,
                IdNegocio = request.IdNegocio
            });
        }

        // Crear venta
        var venta = new Venta
        {
            IdCliente = request.SaleDto.IdCliente,
            IdNegocio = request.IdNegocio,
            MetodoPago = request.SaleDto.MetodoPago,
            FechaVenta = DateTime.UtcNow,
            TotalVenta = totalVenta
        };

        await _unitOfWork.SaleRepository.AddAsync(venta);

        foreach (var d in detallesVenta)
            d.IdVenta = venta.Id;

        foreach (var m in movimientos)
            m.IdVenta = venta.Id;

        await _unitOfWork.DetailSaleRepository.AddRangeAsync(detallesVenta);

        foreach (var movimiento in movimientos)
            await _unitOfWork.MovimientoInventarioRepository.AddAsync(movimiento);

        await _unitOfWork.SaveChangesAsync();

        BackgroundJob.Enqueue<IAlertaSistemaService>(s =>
            s.GenerarAlertaPorVentaAsync(venta.Id));

        foreach (var movimiento in movimientos)
        {
            BackgroundJob.Enqueue<IAlertaSistemaService>(s =>
                s.GenerarAlertaPorMovimientoAsync(movimiento.Id));
        }
        return venta.Id;
    }

}