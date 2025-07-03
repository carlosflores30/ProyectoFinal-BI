using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Providers;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Providers.Queries;

public record GetAllMyProveedoresQuery(int IdNegocio) : IRequest<IEnumerable<ProveedorDto>>;

public class GetAllMyProveedoresQueryHandler : IRequestHandler<GetAllMyProveedoresQuery, IEnumerable<ProveedorDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMyProveedoresQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProveedorDto>> Handle(GetAllMyProveedoresQuery request, CancellationToken cancellationToken)
    {
        var lista = await _unitOfWork.ProveedorRepository.GetAllByNegocioAsync(request.IdNegocio);
        return _mapper.Map<IEnumerable<ProveedorDto>>(lista);
    }
}