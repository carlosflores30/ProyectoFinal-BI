using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Clients;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Clients.Queries;

public record GetAllMyClientesQuery() : IRequest<IEnumerable<ClienteDto>>;

public class GetAllMyClientesQueryHandler : IRequestHandler<GetAllMyClientesQuery, IEnumerable<ClienteDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetAllMyClientesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<IEnumerable<ClienteDto>> Handle(GetAllMyClientesQuery request, CancellationToken cancellationToken)
    {
        var negocioID = _userContextService.GetNegocioId();
        var lista = await _unitOfWork.ClienteRepository.GetAllByNegocioAsync(negocioID);
        return _mapper.Map<IEnumerable<ClienteDto>>(lista);
    }
}