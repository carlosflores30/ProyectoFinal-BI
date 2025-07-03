using System;
using System.Collections.Generic;

namespace SmartStockAI.Infrastructure.Persistence.Models;

public partial class Negocios
{
    public int Id { get; set; }

    public string Ruc { get; set; } = null!;

    public string RazonSocial { get; set; } = null!;

    public string? Direccion { get; set; }

    public int? IdUsuario { get; set; }

    public virtual ICollection<Categorias> Categorias { get; set; } = new List<Categorias>();

    public virtual ICollection<Clientes> Clientes { get; set; } = new List<Clientes>();

    public virtual Usuarios? IdUsuarioNavigation { get; set; }

    public virtual ICollection<MovimientosInventario> MovimientosInventario { get; set; } = new List<MovimientosInventario>();

    public virtual ICollection<Notificaciones> Notificaciones { get; set; } = new List<Notificaciones>();

    public virtual ICollection<Productos> Productos { get; set; } = new List<Productos>();

    public virtual ICollection<Proveedores> Proveedores { get; set; } = new List<Proveedores>();

    public virtual ICollection<RecomendacionesIa> RecomendacionesIa { get; set; } = new List<RecomendacionesIa>();

    public virtual ICollection<Ventas> Ventas { get; set; } = new List<Ventas>();
}
