using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Categories;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Categories.Queries;

public record GetMyCategoriaByIdQuery(int Id, int IdNegocio) : IRequest<CategoriaDto?>;

public class GetMyCategoriaByIdQueryHandler : IRequestHandler<GetMyCategoriaByIdQuery, CategoriaDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMyCategoriaByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoriaDto?> Handle(GetMyCategoriaByIdQuery request, CancellationToken cancellationToken)
    {
        var categoria = await _unitOfWork.CategoriaRepository.GetByIdAsync(request.Id);
        if (categoria == null || categoria.IdNegocio != request.IdNegocio)
            return null;

        return _mapper.Map<CategoriaDto>(categoria);
    }
}