namespace SmartStockAI.Application.DTOs.Reports;

public class VentaReportDto
{
    public string Codigo { get; set; }
    public string Cliente { get; set; }
    public decimal Total { get; set; }
    public string MetodoPago { get; set; }
    public DateTime FechaVenta { get; set; }
}