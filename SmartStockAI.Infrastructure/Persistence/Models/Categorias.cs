using System;
using System.Collections.Generic;

namespace SmartStockAI.Infrastructure.Persistence.Models;

public partial class Categorias
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdNegocio { get; set; }

    public virtual Negocios? IdNegocioNavigation { get; set; }

    public virtual ICollection<Productos> Productos { get; set; } = new List<Productos>();
}
