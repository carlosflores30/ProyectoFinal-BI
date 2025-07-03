using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Application.UsesCases.Notifications.Commands;
using SmartStockAI.Application.UsesCases.Notifications.Queries;

namespace SmartStockAI.Api.Controllers.Notifications;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificacionesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificacionesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // 🔔 Obtener todas las notificaciones (con filtro opcional por título)
    [HttpGet("todas")]
    public async Task<IActionResult> GetTodas([FromQuery] string? titulo)
    {
        var result = await _mediator.Send(new GetAllMyNotificacionesQuery(titulo));
        return Ok(result);
    }

    // 🔴 Obtener solo no leídas (con filtro opcional por título)
    [HttpGet("no-leidas")]
    public async Task<IActionResult> GetNoLeidas([FromQuery] string? titulo)
    {
        var result = await _mediator.Send(new GetNotificacionesNoLeidasQuery(titulo));
        return Ok(result);
    }

    // ✅ Obtener solo las leídas (con filtro opcional por título)
    [HttpGet("leidas")]
    public async Task<IActionResult> GetLeidas([FromQuery] string? titulo)
    {
        var result = await _mediator.Send(new GetNotificacionesLeidasQuery(titulo));
        return Ok(result);
    }

    // ✅ Marcar todas como leídas
    [HttpPut("marcar-como-leidas")]
    public async Task<IActionResult> MarcarTodasComoLeidas()
    {
        await _mediator.Send(new MarcarNotificacionesComoLeidasCommand());
        return NoContent();
    }

    // ✅ Marcar una notificación como leída
    [HttpPut("{id}/marcar-como-leida")]
    public async Task<IActionResult> MarcarUnaComoLeida(int id)
    {
        await _mediator.Send(new MarcarNotificacionComoLeidaCommand(id));
        return NoContent();
    }
}