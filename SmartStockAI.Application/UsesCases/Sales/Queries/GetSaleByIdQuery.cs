using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Sales;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Sales.Queries;

public record GetSaleByIdQuery(int IdVenta) : IRequest<SaleDto?>;

public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, SaleDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetSaleByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<SaleDto?> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var venta = await _unitOfWork.SaleRepository.GetByIdWithDetallesAsync(request.IdVenta);
        if (venta == null || venta.IdNegocio != negocioId)
            return null;

        return _mapper.Map<SaleDto>(venta);
    }
}