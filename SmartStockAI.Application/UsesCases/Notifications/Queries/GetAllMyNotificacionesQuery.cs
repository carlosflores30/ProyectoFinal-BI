using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Notifications;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Notifications.Queries;

public record GetAllMyNotificacionesQuery(string? Titulo = null)
    : IRequest<IEnumerable<NotificacionDto>>;
    
public class GetAllMyNotificacionesQueryHandler 
    : IRequestHandler<GetAllMyNotificacionesQuery, IEnumerable<NotificacionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetAllMyNotificacionesQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<IEnumerable<NotificacionDto>> Handle(GetAllMyNotificacionesQuery request, CancellationToken cancellationToken)
    {
        var idNegocio = _userContextService.GetNegocioId();

        var entidadesEf = await _unitOfWork.NotificacionRepository.ObtenerTodasAsync(idNegocio);

        if (!string.IsNullOrWhiteSpace(request.Titulo))
        {
            entidadesEf = entidadesEf
                .Where(n => n.Titulo.Contains(request.Titulo, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return _mapper.Map<IEnumerable<NotificacionDto>>(entidadesEf);
    }
}