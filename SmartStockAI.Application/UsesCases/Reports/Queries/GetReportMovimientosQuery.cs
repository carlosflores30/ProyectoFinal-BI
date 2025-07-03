using MediatR;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Application.Interfaces.Reports;
using SmartStockAI.Infrastructure.Reports.Interfaces;

namespace SmartStockAI.Application.UsesCases.Reports.Queries;

public record GetReportMovimientosQuery() : IRequest<byte[]>;

public class GetReportMovimientosQueryHandler : IRequestHandler<GetReportMovimientosQuery, byte[]>
{
    private readonly IMovimientoInventarioReportService _reportService;
    private readonly IReporteExcelService _excelService;
    private readonly IUserContextService _userContext;

    public GetReportMovimientosQueryHandler(
        IMovimientoInventarioReportService reportService,
        IReporteExcelService excelService,
        IUserContextService userContext)
    {
        _reportService = reportService;
        _excelService = excelService;
        _userContext = userContext;
    }

    public async Task<byte[]> Handle(GetReportMovimientosQuery request, CancellationToken cancellationToken)
    {
        var idNegocio = _userContext.GetNegocioId();
        var movimientos = await _reportService.GetReporteMovimientosAsync(idNegocio);
        return _excelService.GenerarReporteMovimientos(movimientos);
    }
}
