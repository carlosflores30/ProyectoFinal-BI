using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Clients;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.Clients.Entities;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Clients.Commands;

public record CreateClienteCommand(CreateClienteDto ClienteDto) : IRequest<int>;
public class CreateClienteCommandHandler : IRequestHandler<CreateClienteCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;
    public CreateClienteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }
    public async Task<int> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var existentes = await _unitOfWork.ClienteRepository
            .GetAllByNegocioAsync(negocioId);
        if (existentes.Any(c => c.Dni == request.ClienteDto.Dni))
        {
            throw new ApplicationException("Ya existe un cliente con ese DNI.");
        }
        var cliente = _mapper.Map<Cliente>(request.ClienteDto);
        cliente.IdNegocio = negocioId;
        await _unitOfWork.ClienteRepository.AddAsync(cliente);
        await _unitOfWork.SaveChangesAsync();

        return cliente.Id;
    }
}