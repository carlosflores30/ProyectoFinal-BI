using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Products;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Products.Queries;

public record GetMyProductoByIdQuery(int Id, int IdNegocio) :  IRequest<ProductoDto?>;

public class GetMyProductoByIdQueryHandler : IRequestHandler<GetMyProductoByIdQuery, ProductoDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMyProductoByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductoDto?> Handle(GetMyProductoByIdQuery request, CancellationToken cancellationToken)
    {
        var producto = await _unitOfWork.ProductosRepository.GetByIdAsync(request.Id);
        if (producto == null || producto.IdNegocio != request.IdNegocio)
            return null;

        return _mapper.Map<ProductoDto>(producto);
    }
}