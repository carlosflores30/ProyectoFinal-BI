using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Sales;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Sales.Queries;

public record GetAllMySalesQuery(int IdNegocio) : IRequest<IEnumerable<SaleDto>>;

public class GetAllMySalesQueryHandler : IRequestHandler<GetAllMySalesQuery, IEnumerable<SaleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMySalesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SaleDto>> Handle(GetAllMySalesQuery request, CancellationToken cancellationToken)
    {
        var ventas = await _unitOfWork.SaleRepository.GetAllByNegocioAsync(request.IdNegocio);
        return _mapper.Map<IEnumerable<SaleDto>>(ventas);
    }
}