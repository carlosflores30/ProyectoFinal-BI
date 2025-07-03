namespace SmartStockAI.Application.DTOs.Sales;

public class DetailSaleDto
{
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public string? NombreProducto { get; set; }
    public decimal DescuentoAplicado { get; set; }
}