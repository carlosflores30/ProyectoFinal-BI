using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Inventory;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Inventory.Queries;

public record GetAllMovimientosQuery(int IdNegocio) : IRequest<IEnumerable<InventoryDto>>;

public class GetAllMyMovimientosQueryHandler : IRequestHandler<GetAllMovimientosQuery, IEnumerable<InventoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMyMovimientosQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryDto>> Handle(GetAllMovimientosQuery request, CancellationToken cancellationToken)
    {
        var movimientos = await _unitOfWork.MovimientoInventarioRepository.GetAllByNegocioAsync(request.IdNegocio);
        return _mapper.Map<IEnumerable<InventoryDto>>(movimientos);
    }
}