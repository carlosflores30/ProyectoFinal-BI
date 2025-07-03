namespace SmartStockAI.Application.DTOs.Clients;

public class ClienteDto
{
    public int Id { get; set; }
    public string? Dni { get; set; }
    public string? Nombre { get; set; }
    public string? Correo { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
}