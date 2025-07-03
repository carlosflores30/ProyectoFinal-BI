using MediatR;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Clients.Commands;

public record DeleteClienteCommand(int IdCliente, int IdNegocio) : IRequest<bool>;

public class DeleteClienteCommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteClienteCommand, bool>
{
    public async Task<bool> Handle(DeleteClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = await _unitOfWork.ClienteRepository.GetByIdAsync(request.IdCliente);

        if (cliente == null || cliente.IdNegocio != request.IdNegocio)
            return false;

        await _unitOfWork.ClienteRepository.DeleteAsync(cliente.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}