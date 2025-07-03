using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartStockAI.Application.UsesCases.Reports.Queries;

namespace SmartStockAI.Api.Controllers.Reports;

[ApiController]
[Route("api/reportes")]
[Authorize]
public class ReportesController(IMediator _mediator) : ControllerBase
{
    
    [HttpGet("reporte-movimientos")]
    [Authorize]
    public async Task<IActionResult> DescargarReporteMovimientos()
    {
        var result = await _mediator.Send(new GetReportMovimientosQuery());

        return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte_Movimientos.xlsx");

    }

    
    [HttpGet("reporte-productos")]
    [Authorize]
    public async Task<IActionResult> DescargarReporteProductos()
    {
        var result = await _mediator.Send(new GetReportProductosQuery());
        return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte_Productos.xlsx");
    }
    [HttpGet("reporte-clientes")]
    [Authorize]
    public async Task<IActionResult> DescargarReporteClientes()
    {
        var result = await _mediator.Send(new GetReportClientesQuery());

        return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte_Clientes.xlsx");

    }
    
    [HttpGet("reporte-ventas")]
    [Authorize]
    public async Task<IActionResult> DescargarReporteVentas()
    {
        var result = await _mediator.Send(new GetReportVentasQuery());
        return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte_Ventas.xlsx");
    }
    
    [HttpGet("reporte-detalle-ventas")]
    [Authorize]
    public async Task<IActionResult> DescargarDetalleVentas()
    {
        var result = await _mediator.Send(new GetReporteDetalleVentasQuery());
        return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte_Detalle_Ventas.xlsx");
    }
}