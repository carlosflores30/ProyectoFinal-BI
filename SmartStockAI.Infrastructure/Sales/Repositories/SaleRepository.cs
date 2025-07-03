using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Application.DTOs.Reports;
using SmartStockAI.Application.DTOs.Sales;
using SmartStockAI.Application.Interfaces.Reports;
using SmartStockAI.Domain.Sales.Entities;
using SmartStockAI.Domain.Sales.Interfaces;
using SmartStockAI.Infrastructure.Persistence.Context;

namespace SmartStockAI.Infrastructure.Sales.Repositories;

public class SaleRepository : ISaleRepository, IVentaReportService
{
    private readonly SmartStockDbContext _context;
    private readonly IMapper _mapper;

    public SaleRepository(SmartStockDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<VentaReportDto>> GetReporteVentasAsync(int idNegocio)
    {
        var ventas = await _context.Ventas
            .Include(v => v.IdClienteNavigation)
            .Where(v => v.IdNegocio == idNegocio)
            .ToListAsync();

        return _mapper.Map<List<VentaReportDto>>(ventas);
    }

    public async Task AddAsync(Venta venta)
    {
        var model = _mapper.Map<Persistence.Models.Ventas>(venta);
        await _context.Ventas.AddAsync(model);
        await _context.SaveChangesAsync(); 
        venta.Id = model.Id;
    }
    public async Task<IEnumerable<Venta>> GetAllByNegocioAsync(int idNegocio)
    {
        var models = await _context.Ventas
            .Include(v => v.IdClienteNavigation)
            .Include(v => v.DetalleVenta).ThenInclude(d => d.IdProductoNavigation)
            .Where(v => v.IdNegocio == idNegocio)
            .OrderByDescending(v => v.FechaVenta)
            .ToListAsync();

        return _mapper.Map<IEnumerable<Venta>>(models);
    }
    
    public async Task<Venta?> GetByIdWithDetallesAsync(int id)
    {
        var model = await _context.Ventas
            .Include(v => v.IdClienteNavigation)
            .Include(v => v.DetalleVenta).ThenInclude(d => d.IdProductoNavigation) 
            .FirstOrDefaultAsync(v => v.Id == id);

        return model == null ? null : _mapper.Map<Venta>(model);
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _context.Ventas.FindAsync(id);
        if (model != null)
        {
            _context.Ventas.Remove(model);
        }
    }
    
    public async Task<bool> HayVentasRecientesAsync(int idProducto, int idNegocio, DateTime desde)
    {
        return await _context.Ventas
            .Include(v => v.DetalleVenta)
            .AnyAsync(v =>
                v.IdNegocio == idNegocio &&
                v.FechaVenta >= desde &&
                v.DetalleVenta.Any(d => d.IdProducto == idProducto));
    }
    
    public async Task<Venta?> GetByIdAsync(int idVenta)
    {
        var entidad = await _context.Ventas
            .Include(v => v.IdClienteNavigation)
            .Include(v => v.DetalleVenta).ThenInclude(d => d.IdProductoNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == idVenta);

        return _mapper.Map<Venta?>(entidad);
    }

}