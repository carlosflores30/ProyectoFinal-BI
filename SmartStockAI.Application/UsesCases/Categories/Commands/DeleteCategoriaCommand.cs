using AutoMapper;
using MediatR;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Categories.Commands;

public record DeleteCategoriaCommand(int Id) : IRequest<bool>;

public class DeleteCategoriaCommandHandler : IRequestHandler<DeleteCategoriaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _user;

    public DeleteCategoriaCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _user = userContextService;
    }

    public async Task<bool> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
    {
        var idNegocio = _user.GetNegocioId();
        var categoria = await _unitOfWork.CategoriaRepository.GetByIdAsync(request.Id);
        if (categoria == null || categoria.IdNegocio != idNegocio)
            return false;

        await _unitOfWork.CategoriaRepository.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}