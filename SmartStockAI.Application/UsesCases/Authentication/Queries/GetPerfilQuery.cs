using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Authentication.Queries;

public record GetPerfilQuery(int UsuarioId) : IRequest<UsuarioDto>;

public class GetPerfilQueryHandler : IRequestHandler<GetPerfilQuery, UsuarioDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPerfilQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UsuarioDto> Handle(GetPerfilQuery request, CancellationToken cancellationToken)
    {
        var usuario = await _unitOfWork.Users.GetByIdWithNegocioAsync(request.UsuarioId);

        if (usuario == null)
            throw new KeyNotFoundException("Usuario no encontrado");

        return _mapper.Map<UsuarioDto>(usuario);
    }
}
