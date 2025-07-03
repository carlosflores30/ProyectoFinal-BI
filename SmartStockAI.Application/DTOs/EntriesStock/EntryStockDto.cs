namespace SmartStockAI.Application.DTOs.EntriesStock;

public class EntryStockDto
{
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public string Observacion { get; set; } = "Ingreso manual";
}