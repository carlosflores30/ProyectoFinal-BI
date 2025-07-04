using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Products;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Products.Queries;

public record GetAllMyProductosQuery() : IRequest<IEnumerable<ProductoDto>>;

public class GetAllMyProductosQueryHandler : IRequestHandler<GetAllMyProductosQuery, IEnumerable<ProductoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetAllMyProductosQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<IEnumerable<ProductoDto>> Handle(GetAllMyProductosQuery request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var productos = await _unitOfWork.ProductosRepository.GetAllByNegocioAsync(negocioId);
        return _mapper.Map<IEnumerable<ProductoDto>>(productos);
    }
}