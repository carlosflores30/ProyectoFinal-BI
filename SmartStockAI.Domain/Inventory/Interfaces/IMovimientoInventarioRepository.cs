using SmartStockAI.Domain.Inventory.Entities;

namespace SmartStockAI.Domain.Inventory.Interfaces;

public interface IMovimientoInventarioRepository
{
    Task<int> AddAsync(MovimientoInventario movimiento);
    Task<MovimientoInventario?> GetByIdAsync(int id);
    Task<IEnumerable<MovimientoInventario>> GetByVentaIdAsync(int idVenta, int idNegocio);
    Task<IEnumerable<MovimientoInventario>> GetByProductoIdAsync(int idProducto, int idNegocio);
    Task<IEnumerable<MovimientoInventario>> GetAllByNegocioAsync(int idNegocio);
    Task<IEnumerable<MovimientoInventario>> GetByTipoMovimientoAsync(string tipo, int idNegocio);
    Task DeleteByIdAsync(int id);
}