using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Inventory;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Inventory.Queries;

public record GetMovimientosByVentaQuery(int IdVenta, int IdNegocio) : IRequest<IEnumerable<InventoryDto>>;

public class GetMovimientosByVentaQueryHandler : IRequestHandler<GetMovimientosByVentaQuery, IEnumerable<InventoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMovimientosByVentaQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryDto>> Handle(GetMovimientosByVentaQuery request, CancellationToken cancellationToken)
    {
        var movimientos = await _unitOfWork.MovimientoInventarioRepository.GetByVentaIdAsync(request.IdVenta, request.IdNegocio);
        return _mapper.Map<IEnumerable<InventoryDto>>(movimientos);
    }
}