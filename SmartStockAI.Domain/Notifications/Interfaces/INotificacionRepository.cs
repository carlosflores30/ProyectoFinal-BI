using SmartStockAI.Domain.Notifications.Entities;

namespace SmartStockAI.Domain.Notifications.Interfaces;

public interface INotificacionRepository
{
    Task<List<Notificacion>> ObtenerNoLeidasAsync(int idNegocio);
    Task AgregarAsync(Notificacion notificacion);
    Task MarcarComoLeidasAsync(int idNegocio);
    Task MarcarComoLeidaAsync(int id, int idNegocio);
    Task<bool> ExisteNotificacionAsync(int idNegocio, int? idProducto, string titulo);
    Task<List<Notificacion>> ObtenerLeidasAsync(int idNegocio);
    Task<List<Notificacion>> ObtenerTodasAsync(int idNegocio);
}