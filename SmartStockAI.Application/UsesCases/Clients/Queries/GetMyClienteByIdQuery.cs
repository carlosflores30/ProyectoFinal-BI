using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Clients;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Clients.Queries;

public record GetMyClienteByIdQuery(int IdCliente) : IRequest<ClienteDto?>;

public class GetMyClienteByIdQueryHandler : IRequestHandler<GetMyClienteByIdQuery, ClienteDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetMyClienteByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<ClienteDto?> Handle(GetMyClienteByIdQuery request, CancellationToken cancellationToken)
    {
        var negocioID = _userContextService.GetNegocioId();
        var cliente = await _unitOfWork.ClienteRepository.GetByIdAsync(request.IdCliente);

        if (cliente == null || cliente.IdNegocio != negocioID)
            return null;

        return _mapper.Map<ClienteDto>(cliente);
    }
}