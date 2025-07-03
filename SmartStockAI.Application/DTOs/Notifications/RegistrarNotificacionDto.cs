namespace SmartStockAI.Application.DTOs.Notifications;

public class RegistrarNotificacionDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public int? IdNegocio { get; set; }
    public int? IdProducto { get; set; }
    public int? IdMovimiento { get; set; }
    public int? IdVenta { get; set; }
    public int? IdUsuario { get; set; }
}