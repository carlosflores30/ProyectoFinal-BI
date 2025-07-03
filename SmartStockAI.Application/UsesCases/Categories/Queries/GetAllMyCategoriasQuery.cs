using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Categories;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Categories.Queries;

public record GetAllMyCategoriasQuery(int IdNegocio) : IRequest<IEnumerable<CategoriaDto>>;

public class GetAllMyCategoriasQueryHandler : IRequestHandler<GetAllMyCategoriasQuery, IEnumerable<CategoriaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMyCategoriasQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoriaDto>> Handle(GetAllMyCategoriasQuery request, CancellationToken cancellationToken)
    {
        var categorias = await _unitOfWork.CategoriaRepository.GetAllByNegocioAsync(request.IdNegocio);
        return _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
    }
}