using MediatR;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Providers.Commands;

public record DeleteProveedorCommand(int IdProveedor) : IRequest<bool>;

public class DeleteProveedorCommandHandler : IRequestHandler<DeleteProveedorCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public DeleteProveedorCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<bool> Handle(DeleteProveedorCommand request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var proveedor = await _unitOfWork.ProveedorRepository.GetByIdAsync(request.IdProveedor);

        if (proveedor == null || proveedor.IdNegocio != negocioId)
            return false;

        await _unitOfWork.ProveedorRepository.DeleteAsync(proveedor.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}