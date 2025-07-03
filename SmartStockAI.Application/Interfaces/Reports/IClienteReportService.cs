using SmartStockAI.Application.DTOs.Reports;

namespace SmartStockAI.Application.Interfaces.Reports;

public interface IClienteReportService
{
    Task<List<ClienteReportDto>> GetReporteClientesAsync(int idNegocio);
}