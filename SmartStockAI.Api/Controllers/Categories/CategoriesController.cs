using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Application.DTOs.Categories;
using SmartStockAI.Application.UsesCases.Categories.Commands;
using SmartStockAI.Application.UsesCases.Categories.Queries;

namespace SmartStockAI.Api.Controllers.Categories;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CrearCategoria([FromBody] CreateCategoriaDto dto)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new CreateCategoriaCommand(dto, idNegocio);
        var id = await _mediator.Send(command);

        return CreatedAtAction(nameof(ObtenerCategoria), new { id }, new { id });
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> ObtenerCategoria(int id)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var query = new GetMyCategoriaByIdQuery(id, idNegocio);
        var categoria = await _mediator.Send(query);

        return categoria is null ? NotFound() : Ok(categoria);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ObtenerCategorias()
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var query = new GetAllMyCategoriasQuery(idNegocio);
        var categorias = await _mediator.Send(query);

        return Ok(categorias);
    }

    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> ActualizarCategoria(int id, [FromBody] PatchCategoriaDto dto)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new PatchCategoriaCommand(id, idNegocio, dto);
        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> EliminarCategoria(int id)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var command = new DeleteCategoriaCommand(id, idNegocio);
        var result = await _mediator.Send(command);

        return result ? NoContent() : NotFound();
    }
}