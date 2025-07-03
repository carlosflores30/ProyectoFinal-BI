namespace SmartStockAI.Application.DTOs.Negocios;

public class NegocioDto
{
    public int Id { get; set; }
    public string Ruc { get; set; } = default!;
    public string RazonSocial { get; set; } = default!;
    public string? Direccion { get; set; }
}