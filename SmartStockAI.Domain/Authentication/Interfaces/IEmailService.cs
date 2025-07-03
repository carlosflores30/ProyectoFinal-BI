namespace SmartStockAI.Domain.Authentication.Interfaces;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body);
}