namespace SmartStockAI.Domain.Providers.Entities;

public class Proveedor
{
    public int Id { get; set; }
    public string? NombreEmpresa { get; set; }
    public string? Ruc { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Correo { get; set; }
    public int IdNegocio { get; set; }
}