using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Application.DTOs.Sales;
using SmartStockAI.Application.UsesCases.Sales.Commands;
using SmartStockAI.Application.UsesCases.Sales.Queries;

namespace SmartStockAI.Api.Controllers.Sales;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CrearVenta([FromBody] CreateSaleDto dto)
    {
        // Obtener IdNegocio (usuario autenticado)
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var idUsuario = idNegocio; // si usas el mismo claim como usuario actual

        var command = new CreateSaleCommand(dto, idUsuario);
        var id = await _mediator.Send(command);

        return CreatedAtAction(nameof(ObtenerVenta), new { id }, new { id });
    }
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> EliminarVenta(int id)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var idUsuario = idNegocio; // Si estás usando el mismo claim como ID de usuario

        var resultado = await _mediator.Send(new DeleteSaleCommand(id, idUsuario));

        if (!resultado)
            return NotFound("Venta no encontrada o no autorizada.");

        return Ok("Venta eliminada correctamente.");
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ObtenerVentas()
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var query = new GetAllMySalesQuery();

        var ventas = await _mediator.Send(query);
        return Ok(ventas);
    }
    
    [HttpGet("{id}/detalle")]
    [Authorize]
    public async Task<IActionResult> ObtenerVenta(int id)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var query = new GetSaleByIdQuery(id);

        var venta = await _mediator.Send(query);

        if (venta == null)
            return NotFound("Venta no encontrada o no pertenece al negocio.");

        return Ok(venta);
    }

}