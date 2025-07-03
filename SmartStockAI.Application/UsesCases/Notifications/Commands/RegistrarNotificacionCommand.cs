using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Notifications;
using SmartStockAI.Domain.Notifications.Entities;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Notifications.Commands;

public record RegistrarNotificacionCommand(RegistrarNotificacionDto NotificacionDto) : IRequest<Unit>;

public class RegistrarNotificacionCommandHandler : IRequestHandler<RegistrarNotificacionCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegistrarNotificacionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(RegistrarNotificacionCommand request, CancellationToken cancellationToken)
    {
        var entidad = _mapper.Map<Notificacion>(request.NotificacionDto);
    
        if (entidad.IdNegocio <= 0)
            throw new InvalidOperationException("El IdNegocio no es válido o no fue proporcionado.");

        entidad.Fecha = DateTime.UtcNow;
        entidad.Leido = false;

        await _unitOfWork.NotificacionRepository.AgregarAsync(entidad);
        await _unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}