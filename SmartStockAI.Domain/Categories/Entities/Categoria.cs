namespace SmartStockAI.Domain.Categories.Entities;

public class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public int IdNegocio { get; set; }
}