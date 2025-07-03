using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Application.DTOs.Reports;
using SmartStockAI.Application.Interfaces.Reports;
using SmartStockAI.Domain.Inventory.Entities;
using SmartStockAI.Domain.Inventory.Interfaces;
using SmartStockAI.Infrastructure.Persistence.Context;

namespace SmartStockAI.Infrastructure.Inventory.Repositories;

public class MovimientoInventarioRepository : IMovimientoInventarioRepository, IMovimientoInventarioReportService
{
    private readonly SmartStockDbContext _context;
    private readonly IMapper _mapper;

    public MovimientoInventarioRepository(SmartStockDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<MovimientoReportDto>> GetReporteMovimientosAsync(int idNegocio)
{
    var query = _context.MovimientosInventario
        .Include(m => m.IdProductoNavigation)
        .Where(m => m.IdNegocio == idNegocio)
        .AsQueryable();

    var modelos = await query.ToListAsync();
    
    return _mapper.Map<List<MovimientoReportDto>>(modelos);
}


    public async Task<int> AddAsync(MovimientoInventario movimiento)
    {
        var model = _mapper.Map<Persistence.Models.MovimientosInventario>(movimiento);
        await _context.MovimientosInventario.AddAsync(model);
        await _context.SaveChangesAsync(); 
        return model.Id;
    }
    public async Task<MovimientoInventario?> GetByIdAsync(int id)
    {
        var model = await _context.MovimientosInventario
            .Include(m => m.IdProductoNavigation)     // ← asegúrate de usar el nombre correcto
            .Include(m => m.IdUsuarioNavigation)      // ← si lo necesitas
            .Include(m => m.IdVentaNavigation)        // ← si aplica
            .FirstOrDefaultAsync(m => m.Id == id);

        return model == null ? null : _mapper.Map<MovimientoInventario>(model);
    }
    public async Task<IEnumerable<MovimientoInventario>> GetByVentaIdAsync(int idVenta, int idNegocio)
    {
        var modelos = await _context.MovimientosInventario
            .Where(m => m.IdVenta == idVenta && m.IdNegocio == idNegocio)
            .ToListAsync();

        return _mapper.Map<IEnumerable<MovimientoInventario>>(modelos);
    }
    public async Task<IEnumerable<MovimientoInventario>> GetByProductoIdAsync(int idProducto, int idNegocio)
    {
        var modelos = await _context.MovimientosInventario
            .Where(m => m.IdProducto == idProducto && m.IdNegocio == idNegocio)
            .ToListAsync();

        return _mapper.Map<IEnumerable<MovimientoInventario>>(modelos);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entity = await _context.MovimientosInventario.FindAsync(id);
        if (entity != null)
            _context.MovimientosInventario.Remove(entity);
    }
    public async Task<IEnumerable<MovimientoInventario>> GetAllByNegocioAsync(int idNegocio)
    {
        var movimientos = await _context.MovimientosInventario
            .Where(m => m.IdNegocio == idNegocio)
            .OrderByDescending(m => m.FechaMovimiento)
            .ToListAsync();

        return _mapper.Map<IEnumerable<MovimientoInventario>>(movimientos);
    }
    public async Task<IEnumerable<MovimientoInventario>> GetByTipoMovimientoAsync(string tipo, int idNegocio)
    {
        var models = await _context.MovimientosInventario
            .Where(m => m.TipoMovimiento == tipo && m.IdNegocio == idNegocio)
            .ToListAsync();

        return _mapper.Map<IEnumerable<MovimientoInventario>>(models);
    }
}