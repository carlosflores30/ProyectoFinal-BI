using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Negocios;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Negocios.Queries;

public record GetMyNegocioByIdQuery(int IdNegocio, int UsuarioId) : IRequest<NegocioDto?>;

public class GetMyNegocioByIdQueryHandler : IRequestHandler<GetMyNegocioByIdQuery, NegocioDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMyNegocioByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<NegocioDto?> Handle(GetMyNegocioByIdQuery request, CancellationToken cancellationToken)
    {
        var negocio = await _unitOfWork.NegociosRepository.GetByIdAsync(request.IdNegocio);
        
        if (negocio == null || negocio.IdUsuario != request.UsuarioId)
            return null;

        return _mapper.Map<NegocioDto>(negocio);
    }
}
