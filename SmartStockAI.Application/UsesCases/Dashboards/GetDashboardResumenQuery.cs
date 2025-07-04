using MediatR;
using SmartStockAI.Application.DTOs.Dashboard;
using SmartStockAI.Domain.UnitOfWork.Interfaces;
using System.Linq;
using AutoMapper;
using SmartStockAI.Application.Interfaces.Authentication;

namespace SmartStockAI.Application.UsesCases.Dashboards;

public record GetDashboardResumenQuery() : IRequest<ResumenDashboardDto>;

public class GetDashboardResumenQueryHandler : IRequestHandler<GetDashboardResumenQuery, ResumenDashboardDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _contextService;

    public GetDashboardResumenQueryHandler(IUnitOfWork unitOfWork, IUserContextService contextService)
    {
        _unitOfWork = unitOfWork;
        _contextService = contextService;
    }

    public async Task<ResumenDashboardDto> Handle(GetDashboardResumenQuery request, CancellationToken cancellationToken)
    {
        var negocioId = _contextService.GetNegocioId();
        var productos = await _unitOfWork.ProductosRepository.GetAllByNegocioAsync(negocioId);
        var movimientos = await _unitOfWork.MovimientoInventarioRepository.GetAllByNegocioAsync(negocioId);

        var hoy = DateTime.Today;
        var mes = hoy.Month;
        var año = hoy.Year;

        var ventasDelMes = await _unitOfWork.SaleRepository.ObtenerTotalVentasDelMesAsync(negocioId, mes, año);

    
        var productoMasVendido = movimientos
            .Where(m => m.TipoMovimiento == "Venta")
            .GroupBy(m => m.Producto?.Nombre)
            .OrderByDescending(g => g.Sum(x => x.Cantidad))
            .Select(g => g.Key)
            .FirstOrDefault();

        var ultIngreso = movimientos
            .Where(m => m.TipoMovimiento == "Entrada")
            .OrderByDescending(m => m.FechaMovimiento)
            .Select(m => m.FechaMovimiento)
            .FirstOrDefault();

        var stockBajo = productos.Count(p => p.Stock <= p.Umbral);

        var movimientosHoy = movimientos.Count(m => m.FechaMovimiento.Date == hoy);

        // Este dato depende si tienes tabla de usuarios asociados al negocio

        return new ResumenDashboardDto
        {
            TotalProductos = productos.Count(),
            VentasTotalesMes = ventasDelMes,
            ProductoMasVendido = productoMasVendido,
            UltimoIngresoStock = ultIngreso,
            StockBajo = stockBajo,
            MovimientosHoy = movimientosHoy,
        };
    }
}

