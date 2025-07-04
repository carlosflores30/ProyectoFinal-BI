using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Sales;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Sales.Queries;

public record GetAllMySalesQuery() : IRequest<IEnumerable<SaleDto>>;

public class GetAllMySalesQueryHandler : IRequestHandler<GetAllMySalesQuery, IEnumerable<SaleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetAllMySalesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<IEnumerable<SaleDto>> Handle(GetAllMySalesQuery request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var ventas = await _unitOfWork.SaleRepository.GetAllByNegocioAsync(negocioId);
        return _mapper.Map<IEnumerable<SaleDto>>(ventas);
    }
}