using SmartStockAI.Application.DTOs.Reports;

namespace SmartStockAI.Application.Interfaces.Reports;

public interface IDetalleVentaReportService
{
    Task<List<DetalleVentaReportDto>> GetReporteDetalleVentasAsync(int idNegocio);
}