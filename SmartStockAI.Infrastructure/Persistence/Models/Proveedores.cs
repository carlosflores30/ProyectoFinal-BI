using System;
using System.Collections.Generic;

namespace SmartStockAI.Infrastructure.Persistence.Models;

public partial class Proveedores
{
    public int Id { get; set; }

    public string? NombreEmpresa { get; set; }

    public string? Ruc { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public int? IdNegocio { get; set; }

    public virtual Negocios? IdNegocioNavigation { get; set; }
}
