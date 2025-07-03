using System;
using System.Collections.Generic;

namespace SmartStockAI.Infrastructure.Persistence.Models;

public partial class Notificaciones
{
    public int Id { get; set; }

    public int? IdNegocio { get; set; }

    public string Titulo { get; set; } = null!;

    public string Mensaje { get; set; } = null!;

    public bool? Leido { get; set; }

    public DateTime? Fecha { get; set; }
    public int? IdProducto { get; set; }

    public int? IdMovimiento { get; set; }

    public int? IdVenta { get; set; }

    public int? IdUsuario { get; set; }
    public virtual Negocios? IdNegocioNavigation { get; set; }
    public virtual Productos? IdProductoNavigation { get; set; }
    public virtual MovimientosInventario? IdMovimientoNavigation { get; set; }
    public virtual Ventas? IdVentaNavigation { get; set; }
    public virtual Usuarios? IdUsuarioNavigation { get; set; }
}
