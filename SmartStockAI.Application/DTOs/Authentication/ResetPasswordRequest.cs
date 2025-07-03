namespace SmartStockAI.Application.DTOs.Authentication;

public class ResetPasswordRequest
{
    public string Token { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}