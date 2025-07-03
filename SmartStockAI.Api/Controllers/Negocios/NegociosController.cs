using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Application.DTOs.Negocios;
using SmartStockAI.Application.UsesCases.Negocios.Commands;
using SmartStockAI.Application.UsesCases.Negocios.Queries;

namespace SmartStockAI.Api.Controllers.Negocios;

[ApiController]
[Route("/api/[controller]")]
[Authorize]
public class NegociosController(IMediator _mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CrearNegocio([FromBody] CrearNegocioDto dto)
    {
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new CreateNegocioCommand(dto, usuarioId);

        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(ObtenerNegocio), new { id }, new { id });
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerNegocio(int id)
    {
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var query = new GetMyNegocioByIdQuery(id, usuarioId);
        var negocio = await _mediator.Send(query);

        if (negocio == null)
            return NotFound();

        return Ok(negocio);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ObtenerMisNegocios()
    {
        var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var query = new GetAllMyNegociosQuery(usuarioId);
        var negocios = await _mediator.Send(query);

        return Ok(negocios);
    }

}