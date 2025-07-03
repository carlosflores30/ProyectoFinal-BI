namespace SmartStockAI.Application.DTOs.Reports;

public class DetalleVentaReportDto
{
    public int IdVenta { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal DescuentoAplicado { get; set; }
    public decimal TotalItem { get; set; }
    public DateTime Fecha { get; set; }
}