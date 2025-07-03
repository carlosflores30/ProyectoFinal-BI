using System;
using System.Collections.Generic;

namespace SmartStockAI.Infrastructure.Persistence.Models;

public partial class Productos
{
    public int Id { get; set; }

    public string? CodProducto { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? Stock { get; set; }

    public int? Umbral { get; set; }

    public decimal? PrecioVenta { get; set; }

    public decimal? PrecioCompra { get; set; }

    public decimal? PrecioDescuento { get; set; }

    public DateOnly? FechaIngreso { get; set; }

    public int? IdCategoria { get; set; }

    public int? IdNegocio { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual Categorias? IdCategoriaNavigation { get; set; }

    public virtual Negocios? IdNegocioNavigation { get; set; }

    public virtual ICollection<MovimientosInventario> MovimientosInventario { get; set; } = new List<MovimientosInventario>();
}
