using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Negocios;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Negocios.Queries;

public record GetAllMyNegociosQuery(int UsuarioId) : IRequest<IEnumerable<NegocioDto>>;

public class GetAllMyNegociosQueryHandler : IRequestHandler<GetAllMyNegociosQuery, IEnumerable<NegocioDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllMyNegociosQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NegocioDto>> Handle(GetAllMyNegociosQuery request, CancellationToken cancellationToken)
    {
        var negocios = await _unitOfWork.NegociosRepository.GetAllByUsuarioAsync(request.UsuarioId);
        return _mapper.Map<IEnumerable<NegocioDto>>(negocios);
    }
}