using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Categories;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Categories.Commands;

public record PatchCategoriaCommand(int Id, PatchCategoriaDto CategoriaDto) : IRequest<bool>;

public class PatchCategoriaCommandHandler : IRequestHandler<PatchCategoriaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public PatchCategoriaCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<bool> Handle(PatchCategoriaCommand request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var categoria = await _unitOfWork.CategoriaRepository.GetByIdAsync(request.Id);
        if (categoria == null || categoria.IdNegocio != negocioId)
            return false;

        if (!string.IsNullOrWhiteSpace(request.CategoriaDto.Nombre))
            categoria.Nombre = request.CategoriaDto.Nombre;

        await _unitOfWork.CategoriaRepository.PatchAsync(categoria);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}