namespace SmartStockAI.Application.DTOs.Reports;

public class ClienteReportDto
{
    public string Nombre { get; set; } = null!;
    public string Documento { get; set; } = null!;
    public string Telefono { get; set; } = null!;
    public string? Correo { get; set; }
    public string? Direccion { get; set; }
}