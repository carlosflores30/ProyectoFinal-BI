using SmartStockAI.Domain.Authentication.Entities;

namespace SmartStockAI.Domain.Negocios.Entities;

public class Negocio
{
    public int Id { get; set; }
    public string Ruc { get; set; } = default!;
    public string RazonSocial { get; set; } = default!;
    public string? Direccion { get; set; }
    public int IdUsuario { get; set; }  // ID del usuario propietario

    public Usuario? Usuario { get; set; }
}