using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Inventory;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Inventory.Queries;

public record GetMovimientosByProductoQuery( int IdProducto) : IRequest<IEnumerable<InventoryDto>>;

public class GetMovimientosByProductoQueryHandler : IRequestHandler<GetMovimientosByProductoQuery, IEnumerable<InventoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetMovimientosByProductoQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<IEnumerable<InventoryDto>> Handle(GetMovimientosByProductoQuery request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var movimientos = await _unitOfWork.MovimientoInventarioRepository.GetByProductoIdAsync(request.IdProducto, negocioId);
        return _mapper.Map<IEnumerable<InventoryDto>>(movimientos);
    }
}