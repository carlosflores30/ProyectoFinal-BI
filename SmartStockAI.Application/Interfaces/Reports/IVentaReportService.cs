using SmartStockAI.Application.DTOs.Reports;

namespace SmartStockAI.Application.Interfaces.Reports;

public interface IVentaReportService
{
    Task<List<VentaReportDto>> GetReporteVentasAsync(int idNegocio);
}