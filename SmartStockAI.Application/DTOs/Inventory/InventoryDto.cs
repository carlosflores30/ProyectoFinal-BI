namespace SmartStockAI.Application.DTOs.Inventory;

public class InventoryDto
{
    public int Id { get; set; }
    public int IdProducto { get; set; }
    public string TipoMovimiento { get; set; } = default!;
    public int Cantidad { get; set; }
    public DateTime FechaMovimiento { get; set; }
    public string? Observacion { get; set; }
    public int IdUsuario { get; set; }
    public int? IdVenta { get; set; }
    public int IdNegocio { get; set; }
}