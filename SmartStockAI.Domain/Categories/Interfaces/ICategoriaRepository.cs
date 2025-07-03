using SmartStockAI.Domain.Categories.Entities;

namespace SmartStockAI.Domain.Categories.Interfaces;

public interface ICategoriaRepository
{
    Task<Categoria?> GetByIdAsync(int id);
    Task<IEnumerable<Categoria>> GetAllByNegocioAsync(int idNegocio);
    Task AddAsync(Categoria categoria);
    Task PatchAsync(Categoria categoria);
    Task DeleteAsync(int id);
}