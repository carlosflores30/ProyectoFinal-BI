using SmartStockAI.Domain.Products.Entities;

namespace SmartStockAI.Domain.Products.Interfaces;

public interface IProductoRepository
{
    Task<Producto?> GetByIdAsync(int id);
    Task<IEnumerable<Producto>> GetAllByNegocioAsync(int idNegocio);
    Task AddAsync(Producto producto);
    Task PatchAsync(Producto producto);
    Task DeleteAsync(int id);
    void Update(Producto producto);
}