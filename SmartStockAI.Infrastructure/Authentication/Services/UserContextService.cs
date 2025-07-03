using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SmartStockAI.Application.Interfaces.Authentication;

namespace SmartStockAI.Infrastructure.Authentication.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetNegocioId()
    {
        var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null ? int.Parse(claim.Value) : throw new UnauthorizedAccessException("No se encontró el ID del negocio");
    }

    public int GetUsuarioId()
    {
        var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);
        return claim != null ? int.Parse(claim.Value) : throw new UnauthorizedAccessException("No se encontró el ID del usuario");
    }

    public string? GetEmail()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
    }

    public string? GetRol()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
    }
}