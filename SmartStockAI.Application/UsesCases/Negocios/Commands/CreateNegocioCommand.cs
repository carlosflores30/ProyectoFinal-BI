using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Negocios;
using SmartStockAI.Domain.Negocios.Entities;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Negocios.Commands;

public record CreateNegocioCommand(CrearNegocioDto NegocioDto, int UsuarioId) : IRequest<int>;

public class CreateNegocioCommandHandler : IRequestHandler<CreateNegocioCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateNegocioCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateNegocioCommand request, CancellationToken cancellationToken)
    {
        var negocio = _mapper.Map<Negocio>(request.NegocioDto);
        negocio.IdUsuario = request.UsuarioId;

        await _unitOfWork.NegociosRepository.AddAsync(negocio);
        await _unitOfWork.SaveChangesAsync();

        return negocio.Id;
    }
}
