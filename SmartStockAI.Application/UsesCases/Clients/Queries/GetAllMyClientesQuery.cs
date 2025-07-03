using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Clients;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Clients.Queries;

public record GetAllMyClientesQuery(int IdNegocio) : IRequest<IEnumerable<ClienteDto>>;

public class GetAllMyClientesQueryHandler : IRequestHandler<GetAllMyClientesQuery, IEnumerable<ClienteDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMyClientesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClienteDto>> Handle(GetAllMyClientesQuery request, CancellationToken cancellationToken)
    {
        var lista = await _unitOfWork.ClienteRepository.GetAllByNegocioAsync(request.IdNegocio);
        return _mapper.Map<IEnumerable<ClienteDto>>(lista);
    }
}