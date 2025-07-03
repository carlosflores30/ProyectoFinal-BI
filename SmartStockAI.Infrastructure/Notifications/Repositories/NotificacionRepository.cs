using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Domain.Notifications.Entities;
using SmartStockAI.Domain.Notifications.Interfaces;
using SmartStockAI.Infrastructure.Persistence.Context;
using SmartStockAI.Infrastructure.Persistence.Models;

namespace SmartStockAI.Infrastructure.Notifications.Repositories;

public class NotificacionRepository : INotificacionRepository
{
    private readonly SmartStockDbContext _context;
    private readonly IMapper _mapper;

    public NotificacionRepository(SmartStockDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<Notificacion>> ObtenerNoLeidasAsync(int idNegocio)
    {
        var entidadesEf = await _context.Notificaciones
            .Include(n => n.IdProductoNavigation)
            .Include(n => n.IdMovimientoNavigation)
            .Include(n => n.IdVentaNavigation)
            .Include(n => n.IdUsuarioNavigation)
            .Where(n => n.IdNegocio == idNegocio && n.Leido == false)
            .OrderByDescending(n => n.Fecha)
            .ToListAsync();

        return _mapper.Map<List<Notificacion>>(entidadesEf);
    }

    public async Task AgregarAsync(Notificacion notificacion)
    {
        var entidadEf = _mapper.Map<Notificaciones>(notificacion);
        await _context.Notificaciones.AddAsync(entidadEf);
    }

    public async Task MarcarComoLeidasAsync(int idNegocio)
    {
        await _context.Notificaciones
            .Where(n => n.IdNegocio == idNegocio && n.Leido == false)
            .ExecuteUpdateAsync(n => n.SetProperty(x => x.Leido, true));
    }
    
    public async Task MarcarComoLeidaAsync(int id, int idNegocio)
    {
        await _context.Notificaciones
            .Where(n => n.Id == id && n.IdNegocio == idNegocio && n.Leido == false)
            .ExecuteUpdateAsync(n => n.SetProperty(x => x.Leido, true));
    }

    public async Task<bool> ExisteNotificacionAsync(int idNegocio, int? idProducto, string titulo)
    {
        return await _context.Notificaciones.AnyAsync(n =>
            n.IdNegocio == idNegocio &&
            n.IdProducto == idProducto &&
            n.Titulo == titulo &&
            n.Leido == false);
    }
    
    public async Task<List<Notificacion>> ObtenerLeidasAsync(int idNegocio)
    {
        var entidadesEf = await _context.Notificaciones
            .Include(n => n.IdProductoNavigation)
            .Include(n => n.IdMovimientoNavigation)
            .Include(n => n.IdVentaNavigation)
            .Include(n => n.IdUsuarioNavigation)
            .Where(n => n.IdNegocio == idNegocio && n.Leido == true)
            .OrderByDescending(n => n.Fecha)
            .ToListAsync();

        return _mapper.Map<List<Notificacion>>(entidadesEf);
    }
    
    public async Task<List<Notificacion>> ObtenerTodasAsync(int idNegocio)
    {
        var entidadesEf = await _context.Notificaciones
            .Include(n => n.IdProductoNavigation)
            .Include(n => n.IdMovimientoNavigation)
            .Include(n => n.IdVentaNavigation)
            .Include(n => n.IdUsuarioNavigation)
            .Where(n => n.IdNegocio == idNegocio)
            .OrderByDescending(n => n.Fecha)
            .ToListAsync();

        return _mapper.Map<List<Notificacion>>(entidadesEf);
    }

}