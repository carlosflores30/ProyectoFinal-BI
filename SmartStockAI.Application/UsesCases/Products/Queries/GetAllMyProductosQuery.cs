using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Products;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Products.Queries;

public record GetAllMyProductosQuery(int IdNegocio) : IRequest<IEnumerable<ProductoDto>>;

public class GetAllMyProductosQueryHandler : IRequestHandler<GetAllMyProductosQuery, IEnumerable<ProductoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMyProductosQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductoDto>> Handle(GetAllMyProductosQuery request, CancellationToken cancellationToken)
    {
        var productos = await _unitOfWork.ProductosRepository.GetAllByNegocioAsync(request.IdNegocio);
        return _mapper.Map<IEnumerable<ProductoDto>>(productos);
    }
}