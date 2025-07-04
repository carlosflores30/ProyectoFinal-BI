using MediatR;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Products.Commands;

public record DeleteProductoCommand(int Id) : IRequest<bool>;

public class DeleteProductoCommandHandler : IRequestHandler<DeleteProductoCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public DeleteProductoCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<bool> Handle(DeleteProductoCommand request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var producto = await _unitOfWork.ProductosRepository.GetByIdAsync(request.Id);
        if (producto == null || producto.IdNegocio != negocioId)
            return false;

        await _unitOfWork.ProductosRepository.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}