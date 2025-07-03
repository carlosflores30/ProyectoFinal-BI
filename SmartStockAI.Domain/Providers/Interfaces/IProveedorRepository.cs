using SmartStockAI.Domain.Providers.Entities;

namespace SmartStockAI.Domain.Providers.Interfaces;

public interface IProveedorRepository
{
    Task<Proveedor?> GetByIdAsync(int id);
    Task<IEnumerable<Proveedor>> GetAllByNegocioAsync(int idNegocio);
    Task AddAsync(Proveedor proveedor);
    Task PatchAsync(Proveedor proveedor);
    Task DeleteAsync(int id);
}