using MediatR;
using SmartStockAI.Application.DTOs.Clients;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Clients.Commands;

public record PatchClienteCommand( int IdCliente, PatchClienteDto Dto) : IRequest<bool>;

public class PatchClienteCommandHandler(IUnitOfWork _unitOfWork, IUserContextService _userContextService) : IRequestHandler<PatchClienteCommand, bool>
{

    public async Task<bool> Handle(PatchClienteCommand request, CancellationToken cancellationToken)
    {
        var negocioID = _userContextService.GetNegocioId();
        var cliente = await _unitOfWork.ClienteRepository.GetByIdAsync(request.IdCliente);

        if (cliente == null || cliente.IdNegocio != negocioID)
            return false;

        if (!string.IsNullOrWhiteSpace(request.Dto.Nombre))
            cliente.Nombre = request.Dto.Nombre;

        if (!string.IsNullOrWhiteSpace(request.Dto.Correo))
            cliente.Correo = request.Dto.Correo;

        if (!string.IsNullOrWhiteSpace(request.Dto.Telefono))
            cliente.Telefono = request.Dto.Telefono;

        if (!string.IsNullOrWhiteSpace(request.Dto.Direccion))
            cliente.Direccion = request.Dto.Direccion;

        await _unitOfWork.ClienteRepository.PatchAsync(cliente);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}