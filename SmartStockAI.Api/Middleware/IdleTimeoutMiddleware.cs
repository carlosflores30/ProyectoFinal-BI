using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace SmartStockAI.Api.Middleware;

public class IdleTimeoutMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TimeSpan _idleTimeout = TimeSpan.FromHours(3);
    private static readonly ConcurrentDictionary<string, DateTime> _userLastActivity = new();

    public IdleTimeoutMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();

        // Excluir rutas públicas (sin control de inactividad)
        if (path is not null && (
                path.Contains("/api/auth/login") ||
                path.Contains("/api/auth/register") ||
                path.Contains("/api/auth/forgot-password") ||
                path.Contains("/api/auth/reset-password") ||
                path.Contains("/swagger") ||
                path.Contains("/api-docs")
            ))
        {
            await _next(context);
            return;
        }

        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            await _next(context);
            return;
        }

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (userId == null)
        {
            await _next(context);
            return;
        }

        var now = DateTime.UtcNow;

        if (_userLastActivity.TryGetValue(userId, out var lastActivity))
        {
            if (now - lastActivity > _idleTimeout)
            {
                _userLastActivity.TryRemove(userId, out _);

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                var message = new { error = "Sesión cerrada por inactividad" };
                await context.Response.WriteAsJsonAsync(message);
                return;
            }
        }
        _userLastActivity[userId] = now;

        await _next(context);
    }
}