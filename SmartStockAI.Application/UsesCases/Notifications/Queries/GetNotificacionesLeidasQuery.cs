using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Notifications;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Notifications.Queries;

public record GetNotificacionesLeidasQuery(string? Titulo = null) 
    : IRequest<IEnumerable<NotificacionDto>>;

public class GetNotificacionesLeidasQueryHandler : IRequestHandler<GetNotificacionesLeidasQuery, IEnumerable<NotificacionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetNotificacionesLeidasQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<IEnumerable<NotificacionDto>> Handle(GetNotificacionesLeidasQuery request, CancellationToken cancellationToken)
    {
        var idNegocio = _userContextService.GetNegocioId();
        var notificaciones = await _unitOfWork.NotificacionRepository.ObtenerLeidasAsync(idNegocio);

        if (!string.IsNullOrWhiteSpace(request.Titulo))
        {
            notificaciones = notificaciones
                .Where(n => n.Titulo.Contains(request.Titulo, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return _mapper.Map<IEnumerable<NotificacionDto>>(notificaciones);
    }
}
    