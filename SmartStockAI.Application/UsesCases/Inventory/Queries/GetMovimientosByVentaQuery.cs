using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Inventory;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Inventory.Queries;

public record GetMovimientosByVentaQuery(int IdVenta) : IRequest<IEnumerable<InventoryDto>>;

public class GetMovimientosByVentaQueryHandler : IRequestHandler<GetMovimientosByVentaQuery, IEnumerable<InventoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetMovimientosByVentaQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<IEnumerable<InventoryDto>> Handle(GetMovimientosByVentaQuery request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var movimientos = await _unitOfWork.MovimientoInventarioRepository.GetByVentaIdAsync(request.IdVenta, negocioId);
        return _mapper.Map<IEnumerable<InventoryDto>>(movimientos);
    }
}