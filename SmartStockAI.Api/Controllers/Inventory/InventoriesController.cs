using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Application.UsesCases.Inventory.Queries;

namespace SmartStockAI.Api.Controllers.Inventory;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoriesController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObtenerMovimientos()
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var query = new GetAllMovimientosQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("venta/{idVenta}")]
    public async Task<IActionResult> GetByVenta(int idVenta)
    {
        int idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var query = new GetMovimientosByVentaQuery(idVenta);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("producto/{idProducto}")]
    public async Task<IActionResult> GetByProducto(int idProducto)
    {
        int idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var query = new GetMovimientosByProductoQuery( idProducto);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("tipo/{tipo}")]
    public async Task<IActionResult> GetByTipo([FromRoute] string tipo)
    {
        int idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var query = new GetMovimientosByTipoQuery(tipo);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}