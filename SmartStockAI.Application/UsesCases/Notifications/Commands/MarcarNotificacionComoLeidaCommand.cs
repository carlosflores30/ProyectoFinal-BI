using MediatR;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Notifications.Commands;

public record MarcarNotificacionComoLeidaCommand(int id) : IRequest<Unit>;

public class MarcarNotificacionComoLeidaCommandHandler 
    : IRequestHandler<MarcarNotificacionComoLeidaCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public MarcarNotificacionComoLeidaCommandHandler(
        IUnitOfWork unitOfWork,
        IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<Unit> Handle(MarcarNotificacionComoLeidaCommand request, CancellationToken cancellationToken)
    {
        var idNegocio = _userContextService.GetNegocioId();

        await _unitOfWork.NotificacionRepository
            .MarcarComoLeidaAsync(request.id, idNegocio);

        return Unit.Value;
    }
}