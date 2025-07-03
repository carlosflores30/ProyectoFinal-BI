using System;
using System.Collections.Generic;

namespace SmartStockAI.Infrastructure.Persistence.Models;

public partial class MovimientosInventario
{
    public int Id { get; set; }

    public int? IdProducto { get; set; }

    public string? TipoMovimiento { get; set; }

    public int? Cantidad { get; set; }

    public DateTime? FechaMovimiento { get; set; }

    public string? Observacion { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdVenta { get; set; }

    public int? IdNegocio { get; set; }

    public virtual Negocios? IdNegocioNavigation { get; set; }

    public virtual Productos? IdProductoNavigation { get; set; }

    public virtual Usuarios? IdUsuarioNavigation { get; set; }

    public virtual Ventas? IdVentaNavigation { get; set; }
}
