namespace SmartStockAI.Domain.Notifications.Entities;

public class Notificacion
{
    public int Id { get; set; }
    public int? IdNegocio { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public bool Leido { get; set; } = false;
    public DateTime Fecha { get; set; }
    public int? IdProducto { get; set; }

    public int? IdMovimiento { get; set; }

    public int? IdVenta { get; set; }

    public int? IdUsuario { get; set; }
}