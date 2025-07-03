namespace SmartStockAI.Application.DTOs.Reports;

public class MovimientoReportDto
{
    public string NombreProducto { get; set; }
    public string TipoMovimiento { get; set; }
    public int Cantidad { get; set; }
    public DateTime Fecha { get; set; }
    public string? Observacion { get; set; }
}