using MediatR;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Clients.Commands;

public record DeleteClienteCommand(int IdCliente) : IRequest<bool>;

public class DeleteClienteCommandHandler(IUnitOfWork _unitOfWork, IUserContextService _userContextService) : IRequestHandler<DeleteClienteCommand, bool>
{
    public async Task<bool> Handle(DeleteClienteCommand request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var cliente = await _unitOfWork.ClienteRepository.GetByIdAsync(request.IdCliente);

        if (cliente == null || cliente.IdNegocio != negocioId)
            return false;

        await _unitOfWork.ClienteRepository.DeleteAsync(cliente.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}