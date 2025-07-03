namespace SmartStockAI.Domain.Authentication.Entities;

public class PasswordResetToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime Expiration { get; set; }
    public int UserId { get; set; }
}