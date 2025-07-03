using MediatR;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Categories.Commands;

public record DeleteCategoriaCommand(int Id, int IdNegocio) : IRequest<bool>;

public class DeleteCategoriaCommandHandler : IRequestHandler<DeleteCategoriaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoriaCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = await _unitOfWork.CategoriaRepository.GetByIdAsync(request.Id);
        if (categoria == null || categoria.IdNegocio != request.IdNegocio)
            return false;

        await _unitOfWork.CategoriaRepository.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}