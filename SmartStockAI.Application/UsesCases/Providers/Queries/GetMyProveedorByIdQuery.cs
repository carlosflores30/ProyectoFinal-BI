using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Providers;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Providers.Queries;

public record GetMyProveedorByIdQuery(int IdProveedor) : IRequest<ProveedorDto?>;

public class GetMyProveedorByIdQueryHandler : IRequestHandler<GetMyProveedorByIdQuery, ProveedorDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetMyProveedorByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<ProveedorDto?> Handle(GetMyProveedorByIdQuery request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var proveedor = await _unitOfWork.ProveedorRepository.GetByIdAsync(request.IdProveedor);

        if (proveedor == null || proveedor.IdNegocio != negocioId)
            return null;

        return _mapper.Map<ProveedorDto>(proveedor);
    }
}