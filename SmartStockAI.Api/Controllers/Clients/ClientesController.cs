using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Application.DTOs.Clients;
using SmartStockAI.Application.UsesCases.Clients.Commands;
using SmartStockAI.Application.UsesCases.Clients.Queries;

namespace SmartStockAI.Api.Controllers.Clients;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientesController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CrearCliente([FromBody] CreateClienteDto dto)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new CreateClienteCommand(dto, idNegocio);
        var id = await _mediator.Send(command);

        return CreatedAtAction(nameof(ObtenerCliente), new { id }, new { id });
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> ObtenerCliente(int id)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var query = new GetMyClienteByIdQuery(id, idNegocio);
        var cliente = await _mediator.Send(query);

        return cliente is null ? NotFound() : Ok(cliente);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ObtenerClientes()
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var query = new GetAllMyClientesQuery(idNegocio);
        var clientes = await _mediator.Send(query);

        return Ok(clientes);
    }
    
    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> ActualizarCliente(int id, [FromBody] PatchClienteDto dto)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new PatchClienteCommand(id, idNegocio, dto);
        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }
    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> EliminarCliente(int id)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new DeleteClienteCommand(id, idNegocio);
        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }


}