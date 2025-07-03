using MediatR;
using SmartStockAI.Application.DTOs.Categories;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Categories.Commands;

public record PatchCategoriaCommand(int Id, int IdNegocio, PatchCategoriaDto CategoriaDto) : IRequest<bool>;

public class PatchCategoriaCommandHandler : IRequestHandler<PatchCategoriaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public PatchCategoriaCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(PatchCategoriaCommand request, CancellationToken cancellationToken)
    {
        var categoria = await _unitOfWork.CategoriaRepository.GetByIdAsync(request.Id);
        if (categoria == null || categoria.IdNegocio != request.IdNegocio)
            return false;

        if (!string.IsNullOrWhiteSpace(request.CategoriaDto.Nombre))
            categoria.Nombre = request.CategoriaDto.Nombre;

        await _unitOfWork.CategoriaRepository.PatchAsync(categoria);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}