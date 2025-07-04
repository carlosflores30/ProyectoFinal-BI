using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Negocios;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Negocios.Queries;

public record GetMyNegocioByIdQuery(int UsuarioId) : IRequest<NegocioDto?>;

public class GetMyNegocioByIdQueryHandler : IRequestHandler<GetMyNegocioByIdQuery, NegocioDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetMyNegocioByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<NegocioDto?> Handle(GetMyNegocioByIdQuery request, CancellationToken cancellationToken)
    {
        var negocio = await _unitOfWork.NegociosRepository.GetByIdAsync(request.UsuarioId);
        
        if (negocio == null || negocio.IdUsuario != request.UsuarioId)
            return null;

        return _mapper.Map<NegocioDto>(negocio);
    }
}
