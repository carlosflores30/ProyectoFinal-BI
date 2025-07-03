using System;
using System.Collections.Generic;

namespace SmartStockAI.Infrastructure.Persistence.Models;

public partial class Clientes
{
    public int Id { get; set; }

    public string? Dni { get; set; }

    public string? Nombre { get; set; }

    public string? Correo { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public int? IdNegocio { get; set; }

    public virtual Negocios? IdNegocioNavigation { get; set; }

    public virtual ICollection<Ventas> Ventas { get; set; } = new List<Ventas>();
}
