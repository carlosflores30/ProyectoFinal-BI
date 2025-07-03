using Microsoft.Extensions.Configuration;
using SmartStockAI.Application.Interfaces.Authentication;

namespace SmartStockAI.Infrastructure.Authentication.Services;

public class FrontendUrlProvider : IFrontendUrlProvider
{
    private readonly IConfiguration _configuration;

    public FrontendUrlProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetResetPasswordUrl(string token)
    {
        var baseUrl = _configuration["Frontend:BaseUrl"];
        return $"{baseUrl}/reset-password?token={token}";
    }
}