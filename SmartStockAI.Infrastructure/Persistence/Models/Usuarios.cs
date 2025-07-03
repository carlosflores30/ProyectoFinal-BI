using System;
using System.Collections.Generic;

namespace SmartStockAI.Infrastructure.Persistence.Models;

public partial class Usuarios
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string? Telefono { get; set; }

    public int? IdRol { get; set; }

    public int? IntentosFallidos { get; set; }

    public DateTime? BloqueadoHasta { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Roles? IdRolNavigation { get; set; }

    public virtual ICollection<MovimientosInventario> MovimientosInventario { get; set; } = new List<MovimientosInventario>();

    public virtual Negocios Negocio { get; set; }
    public virtual ICollection<PasswordResetTokens> PasswordResetTokens { get; set; } = new List<PasswordResetTokens>();
    
}
