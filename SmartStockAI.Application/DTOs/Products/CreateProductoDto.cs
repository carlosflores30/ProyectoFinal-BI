namespace SmartStockAI.Application.DTOs.Products;

public class CreateProductoDto
{
    public string CodProducto { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public int Stock { get; set; }
    public int Umbral { get; set; }
    public decimal PrecioVenta { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioDescuento { get; set; }
    public DateOnly FechaIngreso { get; set; }
    public int IdCategoria { get; set; }
}