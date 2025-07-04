using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Application.DTOs.Authentication;
using SmartStockAI.Application.UsesCases.Authentication.Commands;
using SmartStockAI.Application.UsesCases.Authentication.Queries;

namespace SmartStockAI.Api.Controllers.Authentication;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator _mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = new RegisterCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result); 
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var command = new LoginCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result); 
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { errores = new[] { ex.Message } });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { errores = new[] { $"Error interno: {ex.Message}" } });
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var command = new ResetPasswordCommand(request.Token, request.NewPassword);
        await _mediator.Send(command);
        return Ok(new { message = "Contraseña restablecida exitosamente." });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var command = new ForgotPasswordCommand(request.Email);
        await _mediator.Send(command);
        return Ok(new { message = "Se ha enviado un enlace para restablecer tu contraseña si el correo es válido." });
    }
    
    [HttpGet("perfil")]
    public async Task<IActionResult> ObtenerMiPerfil()
    {
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var perfil = await _mediator.Send(new GetPerfilQuery(usuarioId));
        return Ok(perfil);
    }

}