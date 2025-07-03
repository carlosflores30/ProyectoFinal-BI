using SmartStockAI.Domain.Sales.Entities;

namespace SmartStockAI.Domain.Sales.Interfaces;

public interface ISaleRepository
{
    Task AddAsync(Venta venta);
    Task<IEnumerable<Venta>> GetAllByNegocioAsync(int idNegocio);
    Task<Venta?> GetByIdWithDetallesAsync(int id);
    Task DeleteAsync(int id);
    Task<bool> HayVentasRecientesAsync(int idProducto, int idNegocio, DateTime desde);
    Task<Venta?> GetByIdAsync(int idVenta);

}