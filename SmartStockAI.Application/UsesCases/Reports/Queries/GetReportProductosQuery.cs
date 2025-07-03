using MediatR;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Application.Interfaces.Reports;
using SmartStockAI.Infrastructure.Reports.Interfaces;

namespace SmartStockAI.Application.UsesCases.Reports.Queries;

public record GetReportProductosQuery : IRequest<byte[]>;

public class GetReportProductosQueryHandler : IRequestHandler<GetReportProductosQuery, byte[]>
{
    private readonly IProductoReportService _productoReportService;
    private readonly IReporteExcelService _reporteExcelService;
    private readonly IUserContextService _userContext;

    public GetReportProductosQueryHandler(
        IProductoReportService productoReportService,
        IReporteExcelService reporteExcelService,
        IUserContextService userContext)
    {
        _productoReportService = productoReportService;
        _reporteExcelService = reporteExcelService;
        _userContext = userContext;
    }

    public async Task<byte[]> Handle(GetReportProductosQuery request, CancellationToken cancellationToken)
    {
        var idNegocio = _userContext.GetNegocioId();
        var productos = await _productoReportService.GetReporteProductosAsync(idNegocio);
        return _reporteExcelService.GenerarReporteProductos(productos);
    }
}
