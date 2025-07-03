using SmartStockAI.Domain.Clients.Entities;

namespace SmartStockAI.Domain.Sales.Entities;

public class Venta
{
    public int Id { get; set; }
    public int IdNegocio { get; set; }
    public int IdCliente { get; set; }
    public DateTime FechaVenta { get; set; }
    public decimal TotalVenta { get; set; }
    public string MetodoPago { get; set; } = default!;
    public Cliente? Cliente { get; set; }
    public List<DetalleDeVenta> DetalleVenta { get; set; } = new();

}