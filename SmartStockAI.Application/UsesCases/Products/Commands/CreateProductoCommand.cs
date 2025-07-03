using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Products;
using SmartStockAI.Domain.Products.Entities;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Products.Commands;

public record CreateProductoCommand(CreateProductoDto ProductoDto, int IdNegocio) : IRequest<int>;

public class CreateProductoCommandHandler : IRequestHandler<CreateProductoCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductoCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateProductoCommand request, CancellationToken cancellationToken)
    {
        var existentes = await _unitOfWork.ProductosRepository.GetAllByNegocioAsync(request.IdNegocio);

        if (existentes.Any(p => p.CodProducto == request.ProductoDto.CodProducto))
            throw new ApplicationException("Ya existe un producto con ese código.");

        var producto = _mapper.Map<Producto>(request.ProductoDto);
        producto.IdNegocio = request.IdNegocio;

        // Asegurar fecha de ingreso si no viene seteada
        if (producto.FechaIngreso == default)
            producto.FechaIngreso = DateOnly.FromDateTime(DateTime.UtcNow);

        await _unitOfWork.ProductosRepository.AddAsync(producto);
        await _unitOfWork.SaveChangesAsync();

        return producto.Id;
    }
}