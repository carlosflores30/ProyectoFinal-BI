using MediatR;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Application.Interfaces.Reports;
using SmartStockAI.Domain.UnitOfWork.Interfaces;
using SmartStockAI.Infrastructure.Reports.Interfaces;

namespace SmartStockAI.Application.UsesCases.Reports.Queries;

public record GetReportVentasQuery() : IRequest<byte[]>;

public class GetReportVentasQueryHandler : IRequestHandler<GetReportVentasQuery, byte[]>
{
    private readonly IVentaReportService _ventaService;
    private readonly IReporteExcelService _excelService;
    private readonly IUserContextService _userContextService;

    public GetReportVentasQueryHandler(
        IVentaReportService ventaService,
        IReporteExcelService excelService, IUserContextService userContextService)
    {
        _ventaService = ventaService;
        _excelService = excelService;
        _userContextService = userContextService;
    }

    public async Task<byte[]> Handle(GetReportVentasQuery request, CancellationToken cancellationToken)
    {
        var idNegocio = _userContextService.GetNegocioId();
        var ventas = await _ventaService.GetReporteVentasAsync(idNegocio);
        return _excelService.GenerarReporteVentas(ventas);
    }
}