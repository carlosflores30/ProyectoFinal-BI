using SmartStockAI.Domain.Products.Entities;

namespace SmartStockAI.Domain.Sales.Entities;

public class DetalleDeVenta
{
    public int Id { get; set; }
    public int IdVenta { get; set; }
    public int IdProducto { get; set; }
    public Producto? Producto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal DescuentoAplicado { get; set; }
}