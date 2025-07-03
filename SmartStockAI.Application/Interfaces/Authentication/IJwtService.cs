using SmartStockAI.Domain.Authentication.Entities;

namespace SmartStockAI.Application.Interfaces.Authentication;

public interface IJwtService
{
    string GenerateToken(int userId, string email, string role);
}