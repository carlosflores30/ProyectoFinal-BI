using MediatR;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Notifications.Commands;

public record MarcarNotificacionesComoLeidasCommand : IRequest<Unit>;

public class MarcarNotificacionesComoLeidasCommandHandler 
    : IRequestHandler<MarcarNotificacionesComoLeidasCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public MarcarNotificacionesComoLeidasCommandHandler(
        IUnitOfWork unitOfWork,
        IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<Unit> Handle(MarcarNotificacionesComoLeidasCommand request, CancellationToken cancellationToken)
    {
        var idNegocio = _userContextService.GetNegocioId();

        await _unitOfWork.NotificacionRepository.MarcarComoLeidasAsync(idNegocio);
        return Unit.Value;
    }
}
