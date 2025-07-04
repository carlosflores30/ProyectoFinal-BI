using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Application.DTOs.Providers;
using SmartStockAI.Application.UsesCases.Providers.Commands;
using SmartStockAI.Application.UsesCases.Providers.Queries;

namespace SmartStockAI.Api.Controllers.Providers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProvidersController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CrearProveedor([FromBody] CreateProveedorDto dto)
    {
        // Obtener el ID del negocio desde el usuario autenticado
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new CreateProveedorCommand(dto);
        var id = await _mediator.Send(command);

        return CreatedAtAction(nameof(ObtenerProveedor), new { id }, new { id });
    }
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> ObtenerProveedor(int id)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var query = new GetMyProveedorByIdQuery(id);
        var proveedor = await _mediator.Send(query);

        return proveedor is null ? NotFound() : Ok(proveedor);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ObtenerProveedores()
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var query = new GetAllMyProveedoresQuery();
        var proveedores = await _mediator.Send(query);

        return Ok(proveedores);
    }
    
    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> ActualizarProveedor(int id, [FromBody] PatchProveedorDto dto)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new PatchProveedorCommand(id, dto);
        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }
    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> EliminarProveedor(int id)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new DeleteProveedorCommand(id);
        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }
}