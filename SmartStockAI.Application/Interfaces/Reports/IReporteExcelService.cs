using SmartStockAI.Application.DTOs.Reports;

namespace SmartStockAI.Infrastructure.Reports.Interfaces;

public interface IReporteExcelService
{
    byte[] GenerarReporteMovimientos(List<MovimientoReportDto> datos);
    byte[] GenerarReporteProductos(List<ProductoReportDto> datos);
    byte[] GenerarReporteClientes(List<ClienteReportDto> datos);
    byte[] GenerarReporteVentas(List<VentaReportDto> datos);
    byte[] GenerarReporteDetalleVentas(List<DetalleVentaReportDto> detalles);

}