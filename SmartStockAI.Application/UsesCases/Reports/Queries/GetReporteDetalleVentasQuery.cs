using MediatR;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Application.Interfaces.Reports;
using SmartStockAI.Domain.UnitOfWork.Interfaces;
using SmartStockAI.Infrastructure.Reports.Interfaces;

namespace SmartStockAI.Application.UsesCases.Reports.Queries;

public record GetReporteDetalleVentasQuery() : IRequest<byte[]>;

public class GetReporteDetalleVentasQueryHandler : IRequestHandler<GetReporteDetalleVentasQuery, byte[]>
{
    private readonly IDetalleVentaReportService _service;
    private readonly IReporteExcelService _excelService;
    private readonly IUserContextService _userContextService;

    public GetReporteDetalleVentasQueryHandler(IDetalleVentaReportService service, IReporteExcelService excelService, IUserContextService userContextService)
    {
        _service = service;
        _excelService = excelService;
        _userContextService = userContextService;
    }

    public async Task<byte[]> Handle(GetReporteDetalleVentasQuery request, CancellationToken cancellationToken)
    {
        var idNegocio = _userContextService.GetNegocioId();
        var data = await _service.GetReporteDetalleVentasAsync(idNegocio);
        return _excelService.GenerarReporteDetalleVentas(data);
    }
}