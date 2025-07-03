namespace SmartStockAI.Application.DTOs.Reports;

public class ProductoReportDto
{
    public string CodProducto { get; set; }
    public string Nombre { get; set; } = default!;
    public string Descripcion { get; set; } = default!;
    public int Stock { get; set; }
    public int Umbral { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public decimal PrecioDescuento { get; set; }

    public string Categoria { get; set; }
    public DateTime FechaIngreso { get; set; }
}