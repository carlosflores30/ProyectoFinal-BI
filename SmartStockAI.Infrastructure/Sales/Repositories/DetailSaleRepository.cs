using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Application.DTOs.Reports;
using SmartStockAI.Application.Interfaces.Reports;
using SmartStockAI.Domain.Sales.Entities;
using SmartStockAI.Domain.Sales.Interfaces;
using SmartStockAI.Infrastructure.Persistence.Context;

namespace SmartStockAI.Infrastructure.Sales.Repositories;

public class DetailSaleRepository : IDetailSaleRepository, IDetalleVentaReportService   
{
    private readonly SmartStockDbContext _context;
    private readonly IMapper _mapper;

    public DetailSaleRepository(SmartStockDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<List<DetalleVentaReportDto>> GetReporteDetalleVentasAsync(int idNegocio)
    {
        var detalles = await _context.DetalleVenta
            .Include(d => d.IdVentaNavigation)
            .ThenInclude(v => v.IdClienteNavigation)
            .Include(d => d.IdProductoNavigation)
            .Where(d => d.IdVentaNavigation.IdNegocio == idNegocio)
            .ToListAsync();

        return _mapper.Map<List<DetalleVentaReportDto>>(detalles);
    }

    public async Task AddRangeAsync(IEnumerable<DetalleDeVenta> detalles)
    {
        var models = _mapper.Map<IEnumerable<Persistence.Models.DetalleVenta>>(detalles);
        await _context.DetalleVenta.AddRangeAsync(models);
    }

    public async Task<IEnumerable<DetalleDeVenta>> GetByVentaIdAsync(int idVenta)
    {
        var models = await _context.DetalleVenta
            .Include(d => d.IdProductoNavigation)
            .Where(d => d.IdVenta == idVenta)
            .ToListAsync();

        return _mapper.Map<IEnumerable<DetalleDeVenta>>(models);
    }
    public async Task DeleteByVentaIdAsync(int idVenta)
    {
        var detalles = await _context.DetalleVenta
            .Where(d => d.IdVenta == idVenta)
            .ToListAsync();

        _context.DetalleVenta.RemoveRange(detalles);
    }
}