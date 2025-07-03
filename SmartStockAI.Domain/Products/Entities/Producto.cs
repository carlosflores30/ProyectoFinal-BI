namespace SmartStockAI.Domain.Products.Entities;

public class Producto
{
    public int Id { get; set; }
    public string CodProducto { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public int Stock { get; set; }
    public int Umbral { get; set; }
    public decimal PrecioVenta { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal? PrecioDescuento { get; set; }
    public DateOnly FechaIngreso { get; set; }

    public int IdCategoria { get; set; }
    public int IdNegocio { get; set; }
    public string? NombreCategoria { get; set; } // solo para lectura, no se mapea a DB

}