namespace SmartStockAI.Application.DTOs.Notifications;

public class NotificacionDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public bool Leido { get; set; }
}