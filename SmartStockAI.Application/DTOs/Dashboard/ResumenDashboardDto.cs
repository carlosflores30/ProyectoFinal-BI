namespace SmartStockAI.Application.DTOs.Dashboard;

public class ResumenDashboardDto
{
    public int TotalProductos { get; set; }
    public decimal VentasTotalesMes { get; set; }
    public string? ProductoMasVendido { get; set; }
    public DateTime? UltimoIngresoStock { get; set; }
    public int StockBajo { get; set; }
    public int MovimientosHoy { get; set; }
}