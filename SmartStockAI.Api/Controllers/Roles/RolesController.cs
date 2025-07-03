using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Application.DTOs.Roles;
using SmartStockAI.Application.UsesCases.Roles.Commands;
using SmartStockAI.Application.UsesCases.Roles.Queries;
using SmartStockAI.Domain.Roles.Entities;

namespace SmartStockAI.Api.Controllers.Roles;

[ApiController]
[Route("api/[controller]")]
public class RolesController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Role>>> GetAll()
    {
        var roles = await _mediator.Send(new GetAllRolesQuery());
        return Ok(roles);
    }
    
    [HttpPost]
    public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleRequest request)
    {
        var result = await _mediator.Send(new CreateRoleCommand(request));
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteRoleCommand(id));
        return NoContent();
    }
}