using MediatR;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Sales.Commands;

public record DeleteSaleCommand(int IdVenta, int IdUsuario) : IRequest<bool>;

public class DeleteSaleCommandHandler : IRequestHandler<DeleteSaleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContextService _userContextService;

    public DeleteSaleCommandHandler(IUnitOfWork unitOfWork, IUserContextService userContextService)
    {
        _unitOfWork = unitOfWork;
        _userContextService = userContextService;
    }

    public async Task<bool> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        var negocioId = _userContextService.GetNegocioId();
        // Validar que la venta exista y pertenezca al negocio autenticado
        var venta = await _unitOfWork.SaleRepository.GetByIdWithDetallesAsync(request.IdVenta);
        if (venta == null || venta.IdNegocio != negocioId)
            return false;

        // Revertir el stock de los productos relacionados al detalle de venta
        var detalles = await _unitOfWork.DetailSaleRepository.GetByVentaIdAsync(request.IdVenta);
        foreach (var d in detalles)
        {
            var producto = await _unitOfWork.ProductosRepository.GetByIdAsync(d.IdProducto);
            if (producto != null)
            {
                producto.Stock += d.Cantidad; // Reponer stock
                _unitOfWork.ProductosRepository.Update(producto);
            }
        }

        // Eliminar movimientos de inventario relacionados a esta venta
        var movimientos = await _unitOfWork.MovimientoInventarioRepository.GetByVentaIdAsync(request.IdVenta, negocioId);
        foreach (var movimiento in movimientos)
        {
            await _unitOfWork.MovimientoInventarioRepository.DeleteByIdAsync(movimiento.Id);
        }
        
        // Eliminar detalle de venta y venta
        await _unitOfWork.DetailSaleRepository.DeleteByVentaIdAsync(request.IdVenta);
        await _unitOfWork.SaleRepository.DeleteAsync(request.IdVenta);

        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}