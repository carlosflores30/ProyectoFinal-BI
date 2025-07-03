namespace SmartStockAI.Application.DTOs.Authentication;

public class LoginResponse
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string? Role { get; set; }
}