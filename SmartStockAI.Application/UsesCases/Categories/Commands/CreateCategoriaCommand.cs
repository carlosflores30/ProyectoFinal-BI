using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Categories;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.Categories.Entities;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Categories.Commands;

public record CreateCategoriaCommand(CreateCategoriaDto CategoriaDto) : IRequest<int>;

public class CreateCategoriaCommandHandler : IRequestHandler<CreateCategoriaCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public CreateCategoriaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<int> Handle(CreateCategoriaCommand request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        // Verificar duplicado por nombre dentro del negocio
        var existentes = await _unitOfWork.CategoriaRepository.GetAllByNegocioAsync(negocioId);
        if (existentes.Any(c => c.Nombre.ToLower() == request.CategoriaDto.Nombre.ToLower()))
        {
            throw new ApplicationException("Ya existe una categoría con ese nombre.");
        }
        
        var categoria = _mapper.Map<Categoria>(request.CategoriaDto);
        categoria.IdNegocio = negocioId;

        await _unitOfWork.CategoriaRepository.AddAsync(categoria);
        await _unitOfWork.SaveChangesAsync();

        return categoria.Id;
    }
}