using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Application.DTOs.Products;
using SmartStockAI.Application.UsesCases.Products.Commands;
using SmartStockAI.Application.UsesCases.Products.Queries;

namespace SmartStockAI.Api.Controllers.Products;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CrearProducto([FromBody] CreateProductoDto dto)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new CreateProductoCommand(dto);
        var id = await _mediator.Send(command);

        return CreatedAtAction(nameof(ObtenerProducto), new { id }, new { id });
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerProductos()
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var query = new GetAllMyProductosQuery();
        var productos = await _mediator.Send(query);

        return Ok(productos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerProducto(int id)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var query = new GetMyProductoByIdQuery(id);
        var producto = await _mediator.Send(query);

        return producto is null ? NotFound() : Ok(producto);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> ActualizarProducto(int id, [FromBody] PatchProductoDto dto)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new PatchProductoCommand(id, dto);
        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarProducto(int id)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new DeleteProductoCommand(id);
        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }
}