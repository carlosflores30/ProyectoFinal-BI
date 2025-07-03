using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Providers;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Providers.Queries;

public record GetMyProveedorByIdQuery(int IdProveedor, int IdNegocio) : IRequest<ProveedorDto?>;

public class GetMyProveedorByIdQueryHandler : IRequestHandler<GetMyProveedorByIdQuery, ProveedorDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMyProveedorByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProveedorDto?> Handle(GetMyProveedorByIdQuery request, CancellationToken cancellationToken)
    {
        var proveedor = await _unitOfWork.ProveedorRepository.GetByIdAsync(request.IdProveedor);

        if (proveedor == null || proveedor.IdNegocio != request.IdNegocio)
            return null;

        return _mapper.Map<ProveedorDto>(proveedor);
    }
}