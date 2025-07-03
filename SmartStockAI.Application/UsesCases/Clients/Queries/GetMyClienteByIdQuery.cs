using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Clients;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Clients.Queries;

public record GetMyClienteByIdQuery(int IdCliente, int IdNegocio) : IRequest<ClienteDto?>;

public class GetMyClienteByIdQueryHandler : IRequestHandler<GetMyClienteByIdQuery, ClienteDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMyClienteByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ClienteDto?> Handle(GetMyClienteByIdQuery request, CancellationToken cancellationToken)
    {
        var cliente = await _unitOfWork.ClienteRepository.GetByIdAsync(request.IdCliente);

        if (cliente == null || cliente.IdNegocio != request.IdNegocio)
            return null;

        return _mapper.Map<ClienteDto>(cliente);
    }
}