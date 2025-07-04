using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Categories;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Categories.Queries;

public record GetAllMyCategoriasQuery() : IRequest<IEnumerable<CategoriaDto>>;

public class GetAllMyCategoriasQueryHandler : IRequestHandler<GetAllMyCategoriasQuery, IEnumerable<CategoriaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetAllMyCategoriasQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<IEnumerable<CategoriaDto>> Handle(GetAllMyCategoriasQuery request, CancellationToken cancellationToken)
    {
        var negocioID = _userContextService.GetNegocioId();
        var categorias = await _unitOfWork.CategoriaRepository.GetAllByNegocioAsync(negocioID);
        return _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
    }
}