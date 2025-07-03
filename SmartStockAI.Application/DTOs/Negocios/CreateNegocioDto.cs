namespace SmartStockAI.Application.DTOs.Negocios;

public class CrearNegocioDto
{
    public string Ruc { get; set; } = default!;
    public string RazonSocial { get; set; } = default!;
    public string? Direccion { get; set; }
}