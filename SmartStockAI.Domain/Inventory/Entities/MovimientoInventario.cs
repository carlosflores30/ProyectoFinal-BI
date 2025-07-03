using SmartStockAI.Domain.Products.Entities;

namespace SmartStockAI.Domain.Inventory.Entities;

public class MovimientoInventario
{
    public int Id { get; set; }
    public int IdProducto { get; set; }
    public string TipoMovimiento { get; set; } = default!; // Ej: "Venta"
    public int Cantidad { get; set; }
    public DateTime FechaMovimiento { get; set; }
    public string? Observacion { get; set; }
    public int IdUsuario { get; set; }
    public int? IdVenta { get; set; }
    public int IdNegocio { get; set; }
    public Producto Producto { get; set; } // navegación necesaria
}