using SmartStockAI.Application.DTOs.Reports;

namespace SmartStockAI.Application.Interfaces.Reports;

public interface IMovimientoInventarioReportService
{
    Task<List<MovimientoReportDto>> GetReporteMovimientosAsync(int idNegocio);
}