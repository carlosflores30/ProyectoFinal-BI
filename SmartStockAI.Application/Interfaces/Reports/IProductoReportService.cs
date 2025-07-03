using SmartStockAI.Application.DTOs.Reports;

namespace SmartStockAI.Application.Interfaces.Reports;

public interface IProductoReportService
{
    Task<List<ProductoReportDto>> GetReporteProductosAsync(int idNegocio);
}