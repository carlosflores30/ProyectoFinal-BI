using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Products;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.Products.Entities;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Products.Commands;

public record CreateProductoCommand(CreateProductoDto ProductoDto) : IRequest<int>;

public class CreateProductoCommandHandler : IRequestHandler<CreateProductoCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public CreateProductoCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<int> Handle(CreateProductoCommand request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var existentes = await _unitOfWork.ProductosRepository.GetAllByNegocioAsync(negocioId);

        if (existentes.Any(p => p.CodProducto == request.ProductoDto.CodProducto))
            throw new ApplicationException("Ya existe un producto con ese código.");

        var producto = _mapper.Map<Producto>(request.ProductoDto);
        producto.IdNegocio = negocioId;

        // Asegurar fecha de ingreso si no viene seteada
        if (producto.FechaIngreso == default)
            producto.FechaIngreso = DateOnly.FromDateTime(DateTime.UtcNow);

        await _unitOfWork.ProductosRepository.AddAsync(producto);
        await _unitOfWork.SaveChangesAsync();

        return producto.Id;
    }
}