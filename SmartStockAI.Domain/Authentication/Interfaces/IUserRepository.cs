using SmartStockAI.Domain.Authentication.Entities;

namespace SmartStockAI.Domain.Authentication.Interfaces;

public interface IUserRepository
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<Usuario?> GetByIdAsync(int id);
    Task UpdatePasswordAsync(int userId, string hashedPassword);

    Task AddAsync(Usuario user);
}