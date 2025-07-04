using MediatR;
using SmartStockAI.Application.DTOs.Products;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Products.Commands;

public record PatchProductoCommand(int Id, PatchProductoDto ProductoDto) : IRequest<bool>;

public class PatchProductoCommandHandler : IRequestHandler<PatchProductoCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public PatchProductoCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<bool> Handle(PatchProductoCommand request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        var producto = await _unitOfWork.ProductosRepository.GetByIdAsync(request.Id);
        if (producto == null || producto.IdNegocio != negocioId)
            return false;

        var dto = request.ProductoDto;

        if (!string.IsNullOrWhiteSpace(dto.Nombre)) producto.Nombre = dto.Nombre;
        if (!string.IsNullOrWhiteSpace(dto.Descripcion)) producto.Descripcion = dto.Descripcion;
        if (dto.Stock.HasValue) producto.Stock = dto.Stock.Value;
        if (dto.Umbral.HasValue) producto.Umbral = dto.Umbral.Value;
        if (dto.PrecioVenta.HasValue) producto.PrecioVenta = dto.PrecioVenta.Value;
        if (dto.PrecioCompra.HasValue) producto.PrecioCompra = dto.PrecioCompra.Value;
        if (dto.PrecioDescuento.HasValue) producto.PrecioDescuento = dto.PrecioDescuento.Value;
        if (dto.FechaIngreso.HasValue) producto.FechaIngreso = dto.FechaIngreso.Value;
        if (dto.IdCategoria.HasValue) producto.IdCategoria = dto.IdCategoria.Value;

        await _unitOfWork.ProductosRepository.PatchAsync(producto);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}