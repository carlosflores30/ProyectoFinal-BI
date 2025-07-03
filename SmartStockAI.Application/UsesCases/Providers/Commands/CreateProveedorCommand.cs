using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Providers;
using SmartStockAI.Domain.Providers.Entities;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Providers.Commands;

public record CreateProveedorCommand(CreateProveedorDto ProveedorDto, int IdNegocio) : IRequest<int>;

public class CreateProveedorCommandHandler : IRequestHandler<CreateProveedorCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProveedorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateProveedorCommand request, CancellationToken cancellationToken)
    {
        var proveedor = _mapper.Map<Proveedor>(request.ProveedorDto);
        proveedor.IdNegocio = request.IdNegocio;

        await _unitOfWork.ProveedorRepository.AddAsync(proveedor);
        await _unitOfWork.SaveChangesAsync();

        return proveedor.Id;
    }
}