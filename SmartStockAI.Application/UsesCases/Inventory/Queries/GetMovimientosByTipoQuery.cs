using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Inventory;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Inventory.Queries;

public record GetMovimientosByTipoQuery(string TipoMovimiento, int IdNegocio) : IRequest<IEnumerable<InventoryDto>>;

public class GetMovimientosByTipoQueryHandler : IRequestHandler<GetMovimientosByTipoQuery, IEnumerable<InventoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMovimientosByTipoQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryDto>> Handle(GetMovimientosByTipoQuery request, CancellationToken cancellationToken)
    {
        var movimientos = await _unitOfWork.MovimientoInventarioRepository.GetByTipoMovimientoAsync(request.TipoMovimiento, request.IdNegocio);
        return _mapper.Map<IEnumerable<InventoryDto>>(movimientos);
    }
}