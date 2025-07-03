namespace SmartStockAI.Domain.Clients.Entities;

public class Cliente
{
    public int Id { get; set; }
    public string? Dni { get; set; }
    public string? Nombre { get; set; }
    public string? Correo { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public int IdNegocio { get; set; }
}