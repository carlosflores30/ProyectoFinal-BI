using SmartStockAI.Domain.Negocios.Entities;

namespace SmartStockAI.Domain.Authentication.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Correo { get; set; } = null!;
    public string Contrasena { get; set; } = null!;
    public string? Telefono { get; set; }
    public int IdRol { get; set; }

    public int IntentosFallidos { get; set; }
    public DateTime? BloqueadoHasta { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string? RoleName { get; set; }
    public int? NegocioId { get; set; }

    public Negocio? Negocio { get; set; }

}