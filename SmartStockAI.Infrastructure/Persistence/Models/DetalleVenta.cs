using System;
using System.Collections.Generic;

namespace SmartStockAI.Infrastructure.Persistence.Models;

public partial class DetalleVenta
{
    public int Id { get; set; }

    public int? IdVenta { get; set; }

    public int? IdProducto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? DescuentoAplicado { get; set; }

    public virtual Productos? IdProductoNavigation { get; set; }

    public virtual Ventas? IdVentaNavigation { get; set; }
}
