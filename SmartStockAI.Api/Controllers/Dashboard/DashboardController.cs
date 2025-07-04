using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Application.UsesCases.Dashboards;

namespace SmartStockAI.Api.Controllers.Dashboard;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController(IMediator _mediator) : ControllerBase
{
    [HttpGet("resumen")]
    public async Task<IActionResult> ObtenerResumen()
    {
        var negocioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var resumen = await _mediator.Send(new GetDashboardResumenQuery());
        return Ok(resumen);
    }
}