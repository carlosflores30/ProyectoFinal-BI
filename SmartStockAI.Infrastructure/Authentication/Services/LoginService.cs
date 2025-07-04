using Microsoft.EntityFrameworkCore;
using SmartStockAI.Application.DTOs.Authentication;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Infrastructure.Persistence.Context;

namespace SmartStockAI.Infrastructure.Authentication.Services;

public class LoginService : ILoginService
{
    private readonly SmartStockDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    private const int MaxAttempts = 4;
    private const int LockDurationMinutes = 15;

    public LoginService(
        SmartStockDbContext context,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse> AttemptLoginAsync(string email, string password)
    {
        var user = await _context.Usuarios
            .Include(u => u.IdRolNavigation)
            .FirstOrDefaultAsync(u => u.Correo == email);

        if (user == null)
            throw new UnauthorizedAccessException("Email o contraseña inválidos.");

        if (user.BloqueadoHasta.HasValue && user.BloqueadoHasta > DateTime.UtcNow)
        {
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(user.BloqueadoHasta.Value, TimeZoneInfo.Local);
            throw new UnauthorizedAccessException($"Cuenta bloqueada hasta las {localTime:HH:mm:ss}.");
        }

        var isPasswordValid = _passwordHasher.Verify(password, user.Contrasena);

        if (!isPasswordValid)
        {
            user.IntentosFallidos++;

            if (user.IntentosFallidos >= MaxAttempts)
            {
                user.BloqueadoHasta = DateTime.UtcNow.AddMinutes(LockDurationMinutes);
                user.IntentosFallidos = 0;

                await _context.SaveChangesAsync();
                throw new UnauthorizedAccessException("Cuenta bloqueada. Inténtalo más tarde.");
            }

            await _context.SaveChangesAsync();
            var remaining = MaxAttempts - user.IntentosFallidos;
            throw new UnauthorizedAccessException($"Contraseña inválida. Te quedan {remaining} intento(s).");
        }

        user.IntentosFallidos = 0;
        user.BloqueadoHasta = null;
        await _context.SaveChangesAsync();

        // Validación segura del nombre del rol
        var roleName = user.IdRolNavigation?.Nombre;
        if (string.IsNullOrWhiteSpace(roleName))
            roleName = "User";

        if (user.NegocioId == null)
            throw new Exception("El usuario no tiene asignado un negocio.");

        var token = _jwtService.GenerateToken(
            user.Id,
            user.Correo!,
            roleName,
            user.NegocioId.Value 
        );

        return new LoginResponse
        {
            FullName = $"{user.Nombre} {user.Apellido}",
            Email = user.Correo!,
            Token = token,
            Role = roleName,
            NegocioId = user.NegocioId
        };
    }
}
