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
        var user = _httpContextAccessor.HttpContext?.User;

        // ⚠️ Aquí accede directamente a "sub" (id del usuario), o incluso a un valor de sesión
        var negocioIdStr = user?.Claims.FirstOrDefault(x => x.Type == "idNegocio")?.Value;

        if (string.IsNullOrEmpty(negocioIdStr))
            throw new UnauthorizedAccessException("No se encontró el id del negocio en el contexto.");

        return int.Parse(negocioIdStr);
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