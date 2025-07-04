using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Providers;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.Providers.Entities;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Providers.Commands;

public record CreateProveedorCommand(CreateProveedorDto ProveedorDto) : IRequest<int>;

public class CreateProveedorCommandHandler : IRequestHandler<CreateProveedorCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public CreateProveedorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<int> Handle(CreateProveedorCommand request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var proveedor = _mapper.Map<Proveedor>(request.ProveedorDto);
        proveedor.IdNegocio = negocioId;

        await _unitOfWork.ProveedorRepository.AddAsync(proveedor);
        await _unitOfWork.SaveChangesAsync();

        return proveedor.Id;
    }
}