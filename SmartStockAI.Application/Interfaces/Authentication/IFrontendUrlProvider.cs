namespace SmartStockAI.Application.Interfaces.Authentication;

public interface IFrontendUrlProvider
{
    string GetResetPasswordUrl(string token);
}