namespace SmartStockAI.Application.DTOs.Sales;

public class SaleDto
{
    public int Id { get; set; }
    public int IdCliente { get; set; }
    public string? NombreCliente { get; set; }
    public string MetodoPago { get; set; } = default!;
    public DateTime FechaVenta { get; set; }
    public decimal TotalVenta { get; set; }
    public List<DetailSaleDto> Detalles { get; set; } = new();
}