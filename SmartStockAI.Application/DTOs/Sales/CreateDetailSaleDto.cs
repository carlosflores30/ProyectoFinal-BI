namespace SmartStockAI.Application.DTOs.Sales;

public class CreateDetailSaleDto
{
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal DescuentoAplicado { get; set; }
}