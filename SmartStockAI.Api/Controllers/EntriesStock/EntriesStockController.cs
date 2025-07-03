using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Application.DTOs.EntriesStock;
using SmartStockAI.Application.UsesCases.Inventory.Commands;

namespace SmartStockAI.Api.Controllers.EntriesStock;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EntriesStockController(IMediator _mediator) : Controller
{
    [HttpPost("stock/ingresar")]
    [Authorize]
    public async Task<IActionResult> IngresarStock([FromBody] EntryStockDto dto)
    {
        var idNegocio = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var idUsuario = idNegocio;

        var command = new EntryStockCommand(dto, idNegocio, idUsuario);
        var resultado = await _mediator.Send(command);

        return resultado ? Ok("Stock ingresado correctamente.") : BadRequest("No se pudo ingresar el stock.");
    }
}