using SmartStockAI.Domain.Negocios.Entities;

namespace SmartStockAI.Domain.Negocios.Interfaces;

public interface INegocioRepository
{
    Task<Negocio?> GetByIdAsync(int id);
    Task<IEnumerable<Negocio>> GetAllByUsuarioAsync(int idUsuario);
    Task AddAsync(Negocio negocio);
    void Update(Negocio negocio);
    void Delete(Negocio negocio);
    Task<IEnumerable<Negocio>> ObtenerTodosAsync();
}