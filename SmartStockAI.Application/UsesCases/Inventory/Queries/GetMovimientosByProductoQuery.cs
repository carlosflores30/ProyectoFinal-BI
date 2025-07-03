using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Inventory;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Inventory.Queries;

public record GetMovimientosByProductoQuery(int IdNegocio, int IdProducto) : IRequest<IEnumerable<InventoryDto>>;

public class GetMovimientosByProductoQueryHandler : IRequestHandler<GetMovimientosByProductoQuery, IEnumerable<InventoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMovimientosByProductoQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryDto>> Handle(GetMovimientosByProductoQuery request, CancellationToken cancellationToken)
    {
        var movimientos = await _unitOfWork.MovimientoInventarioRepository.GetByProductoIdAsync(request.IdProducto, request.IdNegocio);
        return _mapper.Map<IEnumerable<InventoryDto>>(movimientos);
    }
}