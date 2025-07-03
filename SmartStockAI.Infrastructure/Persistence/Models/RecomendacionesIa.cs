using System;
using System.Collections.Generic;

namespace SmartStockAI.Infrastructure.Persistence.Models;

public partial class RecomendacionesIa
{
    public int Id { get; set; }

    public int? IdNegocio { get; set; }

    public string Prompt { get; set; } = null!;

    public string Respuesta { get; set; } = null!;

    public DateTime? Fecha { get; set; }

    public virtual Negocios? IdNegocioNavigation { get; set; }
}
