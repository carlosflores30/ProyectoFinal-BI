using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Sales;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Sales.Queries;

public record GetSaleByIdQuery(int IdVenta, int IdNegocio) : IRequest<SaleDto?>;

public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, SaleDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSaleByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SaleDto?> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var venta = await _unitOfWork.SaleRepository.GetByIdWithDetallesAsync(request.IdVenta);
        if (venta == null || venta.IdNegocio != request.IdNegocio)
            return null;

        return _mapper.Map<SaleDto>(venta);
    }
}