using SmartStockAI.Application.DTOs.Authentication;

namespace SmartStockAI.Application.Interfaces.Authentication;

public interface ILoginService
{
    Task<LoginResponse> AttemptLoginAsync(string email, string password);
}