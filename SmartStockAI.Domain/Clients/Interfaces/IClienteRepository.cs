using SmartStockAI.Domain.Clients.Entities;

namespace SmartStockAI.Domain.Clients.Interfaces;

public interface IClienteRepository
{
    Task<Cliente?> GetByIdAsync(int id);
    Task<IEnumerable<Cliente>> GetAllByNegocioAsync(int idNegocio);
    Task AddAsync(Cliente cliente);
    Task PatchAsync(Cliente cliente);
    Task DeleteAsync(int id);
}