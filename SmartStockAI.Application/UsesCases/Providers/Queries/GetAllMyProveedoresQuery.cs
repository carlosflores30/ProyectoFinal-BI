using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Providers;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Providers.Queries;

public record GetAllMyProveedoresQuery() : IRequest<IEnumerable<ProveedorDto>>;

public class GetAllMyProveedoresQueryHandler : IRequestHandler<GetAllMyProveedoresQuery, IEnumerable<ProveedorDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetAllMyProveedoresQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<IEnumerable<ProveedorDto>> Handle(GetAllMyProveedoresQuery request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var lista = await _unitOfWork.ProveedorRepository.GetAllByNegocioAsync(negocioId);
        return _mapper.Map<IEnumerable<ProveedorDto>>(lista);
    }
}