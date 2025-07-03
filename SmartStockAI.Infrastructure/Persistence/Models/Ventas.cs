using System;
using System.Collections.Generic;

namespace SmartStockAI.Infrastructure.Persistence.Models;

public partial class Ventas
{
    public int Id { get; set; }

    public int? IdNegocio { get; set; }

    public int? IdCliente { get; set; }

    public DateTime? FechaVenta { get; set; }

    public decimal? TotalVenta { get; set; }

    public string? MetodoPago { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual Clientes? IdClienteNavigation { get; set; }

    public virtual Negocios? IdNegocioNavigation { get; set; }

    public virtual ICollection<MovimientosInventario> MovimientosInventario { get; set; } = new List<MovimientosInventario>();
}
