namespace SmartStockAI.Application.DTOs.Sales;

public class CreateSaleDto
{
    public int IdCliente { get; set; }
    public string MetodoPago { get; set; } = default!;
    public List<CreateDetailSaleDto> Detalles { get; set; } = new();
}