using SmartStockAI.Domain.Authentication.Entities;

namespace SmartStockAI.Domain.Authentication.Interfaces;

public interface IPasswordResetTokenRepository
{
    Task<PasswordResetToken?> GetByTokenAsync(string token);
    Task AddAsync(PasswordResetToken token);
    Task DeleteAsync(Guid id);
}