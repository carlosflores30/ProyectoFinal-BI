using SmartStockAI.Domain.Roles.Entities;

namespace SmartStockAI.Domain.Roles.Interfaces;

public interface IRoleRepository
{
    Task<List<Role>> GetAllAsync();
    Task AddAsync(Role role);
    Task DeleteAsync(int id);

    Task<Role?> GetByIdAsync(int id);
}