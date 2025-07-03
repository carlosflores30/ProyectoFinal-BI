using MediatR;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Products.Commands;

public record DeleteProductoCommand(int Id, int IdNegocio) : IRequest<bool>;

public class DeleteProductoCommandHandler : IRequestHandler<DeleteProductoCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductoCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteProductoCommand request, CancellationToken cancellationToken)
    {
        var producto = await _unitOfWork.ProductosRepository.GetByIdAsync(request.Id);
        if (producto == null || producto.IdNegocio != request.IdNegocio)
            return false;

        await _unitOfWork.ProductosRepository.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}