using MediatR;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Application.Interfaces.Reports;
using SmartStockAI.Infrastructure.Reports.Interfaces;

namespace SmartStockAI.Application.UsesCases.Reports.Queries;

public record GetReportClientesQuery() : IRequest<byte[]>;

public class GetReportClientesQueryHandler : IRequestHandler<GetReportClientesQuery, byte[]>
{
    private readonly IClienteReportService _clienteService;
    private readonly IReporteExcelService _excelService;
    private readonly IUserContextService _userContext;

    public GetReportClientesQueryHandler(IClienteReportService clienteService, IReporteExcelService excelService, IUserContextService userContext)
    {
        _clienteService = clienteService;
        _excelService = excelService;
        _userContext = userContext;
    }

    public async Task<byte[]> Handle(GetReportClientesQuery request, CancellationToken cancellationToken)
    {
        var idNegocio = _userContext.GetNegocioId();
        var clientes = await _clienteService.GetReporteClientesAsync(idNegocio);
        return _excelService.GenerarReporteClientes(clientes);
    }
}