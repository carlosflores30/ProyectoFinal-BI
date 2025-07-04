using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Products;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Products.Queries;

public record GetMyProductoByIdQuery(int Id) :  IRequest<ProductoDto?>;

public class GetMyProductoByIdQueryHandler : IRequestHandler<GetMyProductoByIdQuery, ProductoDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetMyProductoByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<ProductoDto?> Handle(GetMyProductoByIdQuery request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var producto = await _unitOfWork.ProductosRepository.GetByIdAsync(request.Id);
        if (producto == null || producto.IdNegocio != negocioId)
            return null;

        return _mapper.Map<ProductoDto>(producto);
    }
}