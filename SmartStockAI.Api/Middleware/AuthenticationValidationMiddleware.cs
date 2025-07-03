using System.Text.Json;
using System.Text.RegularExpressions;

namespace SmartStockAI.Api.Middleware;

public class AuthenticationValidationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Post &&
            (context.Request.Path.StartsWithSegments("/api/auth/login") ||
             context.Request.Path.StartsWithSegments("/api/auth/register") ||
             context.Request.Path.StartsWithSegments("/api/auth/reset-password") ||
             context.Request.Path.StartsWithSegments("/api/auth/forgot-password")))
        {
            try
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(
                    context.Request.Body,
                    encoding: System.Text.Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true);

                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                var errors = new List<string>();
                using var doc = JsonDocument.Parse(body);
                var root = doc.RootElement;

                // Validar email y contraseña SOLO en login y register
                if (context.Request.Path.StartsWithSegments("/api/auth/login") ||
                    context.Request.Path.StartsWithSegments("/api/auth/register"))
                {
                    if (!root.TryGetProperty("email", out var emailProp) ||
                        string.IsNullOrWhiteSpace(emailProp.GetString()))
                        errors.Add("El campo 'email' es obligatorio.");
                    else if (!Regex.IsMatch(emailProp.GetString()!, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                        errors.Add("El formato del correo electrónico es inválido.");

                    if (!root.TryGetProperty("password", out var passProp) ||
                        string.IsNullOrWhiteSpace(passProp.GetString()))
                        errors.Add("El campo 'contraseña' es obligatorio.");
                    else
                    {
                        var pwd = passProp.GetString()!;
                        if (pwd.Length < 8 ||
                            !Regex.IsMatch(pwd, @"[a-z]") ||
                            !Regex.IsMatch(pwd, @"[A-Z]") ||
                            !Regex.IsMatch(pwd, @"\d"))
                        {
                            errors.Add(
                                "La contraseña debe tener al menos 8 caracteres, incluyendo letras minúsculas, mayúsculas y números.");
                        }
                    }
                }

                if (context.Request.Path.StartsWithSegments("/api/auth/register"))
                {
                    if (!root.TryGetProperty("firstName", out var fn) || string.IsNullOrWhiteSpace(fn.GetString()))
                        errors.Add("El campo 'nombre' es obligatorio.");
                    else if (fn.GetString()!.Length < 2 || !Regex.IsMatch(fn.GetString()!, @"^[a-zA-Z]+$"))
                        errors.Add("El nombre debe tener al menos 2 letras y contener solo letras.");

                    if (!root.TryGetProperty("lastName", out var ln) || string.IsNullOrWhiteSpace(ln.GetString()))
                        errors.Add("El campo 'apellido' es obligatorio.");
                    else if (ln.GetString()!.Length < 2 || !Regex.IsMatch(ln.GetString()!, @"^[a-zA-Z]+$"))
                        errors.Add("El apellido debe tener al menos 2 letras y contener solo letras.");

                    if (!root.TryGetProperty("phone", out var phone) || string.IsNullOrWhiteSpace(phone.GetString()))
                        errors.Add("El campo 'teléfono' es obligatorio.");
                    else if (!Regex.IsMatch(phone.GetString()!, @"^\d{9}$"))
                        errors.Add("El teléfono debe contener 9 dígitos numéricos.");

                    if (!root.TryGetProperty("roleId", out var role) || role.GetInt32() <= 0)
                        errors.Add("Debe seleccionar un rol válido.");
                }

                if (context.Request.Path.StartsWithSegments("/api/auth/reset-password"))
                {
                    if (!root.TryGetProperty("token", out var tokenProp) ||
                        string.IsNullOrWhiteSpace(tokenProp.GetString()))
                        errors.Add("El token de recuperación es obligatorio.");

                    if (!root.TryGetProperty("newPassword", out var newPassProp) ||
                        string.IsNullOrWhiteSpace(newPassProp.GetString()))
                        errors.Add("La nueva contraseña es obligatoria.");
                    else
                    {
                        var pwd = newPassProp.GetString()!;
                        if (pwd.Length < 8 ||
                            !Regex.IsMatch(pwd, @"[a-z]") ||
                            !Regex.IsMatch(pwd, @"[A-Z]") ||
                            !Regex.IsMatch(pwd, @"\d"))
                        {
                            errors.Add(
                                "La nueva contraseña debe tener al menos 8 caracteres, incluyendo letras minúsculas, mayúsculas y números.");
                        }
                    }
                }

                if (errors.Any())
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new { errors });
                    return;
                }

                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { errores = new[] { ex.Message } });
            }
            catch (InvalidOperationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { errores = new[] { ex.Message } });
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                    { errores = new[] { "Ocurrió un error interno del servidor." } });
            }
        }
        else
        {
            await _next(context);
        }
    }
}
