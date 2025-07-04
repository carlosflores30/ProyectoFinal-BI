using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Categories;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Categories.Queries;

public record GetMyCategoriaByIdQuery(int Id) : IRequest<CategoriaDto?>;

public class GetMyCategoriaByIdQueryHandler : IRequestHandler<GetMyCategoriaByIdQuery, CategoriaDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public GetMyCategoriaByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<CategoriaDto?> Handle(GetMyCategoriaByIdQuery request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var categoria = await _unitOfWork.CategoriaRepository.GetByIdAsync(request.Id);
        if (categoria == null || categoria.IdNegocio != negocioId)
            return null;

        return _mapper.Map<CategoriaDto>(categoria);
    }
}