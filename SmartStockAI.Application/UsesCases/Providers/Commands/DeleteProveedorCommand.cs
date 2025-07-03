using MediatR;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Providers.Commands;

public record DeleteProveedorCommand(int IdProveedor, int IdNegocio) : IRequest<bool>;

public class DeleteProveedorCommandHandler : IRequestHandler<DeleteProveedorCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProveedorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteProveedorCommand request, CancellationToken cancellationToken)
    {
        var proveedor = await _unitOfWork.ProveedorRepository.GetByIdAsync(request.IdProveedor);

        if (proveedor == null || proveedor.IdNegocio != request.IdNegocio)
            return false;

        await _unitOfWork.ProveedorRepository.DeleteAsync(proveedor.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}