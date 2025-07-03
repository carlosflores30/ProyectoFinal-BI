using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Clients;
using SmartStockAI.Domain.Clients.Entities;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Clients.Commands;

public record CreateClienteCommand(CreateClienteDto ClienteDto, int IdNegocio) : IRequest<int>;
public class CreateClienteCommandHandler : IRequestHandler<CreateClienteCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateClienteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<int> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
    {
        var existentes = await _unitOfWork.ClienteRepository
            .GetAllByNegocioAsync(request.IdNegocio);
        if (existentes.Any(c => c.Dni == request.ClienteDto.Dni))
        {
            throw new ApplicationException("Ya existe un cliente con ese DNI.");
        }
        var cliente = _mapper.Map<Cliente>(request.ClienteDto);
        cliente.IdNegocio = request.IdNegocio;
        await _unitOfWork.ClienteRepository.AddAsync(cliente);
        await _unitOfWork.SaveChangesAsync();

        return cliente.Id;
    }
}