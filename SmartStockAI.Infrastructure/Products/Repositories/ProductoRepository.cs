using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Application.DTOs.Reports;
using SmartStockAI.Application.Interfaces.Reports;
using SmartStockAI.Domain.Products.Entities;
using SmartStockAI.Domain.Products.Interfaces;
using SmartStockAI.Infrastructure.Persistence.Context;

namespace SmartStockAI.Infrastructure.Products.Repositories;

public class ProductoRepository : IProductoRepository, IProductoReportService
{
    private readonly SmartStockDbContext _context;
    private readonly IMapper _mapper;

    public ProductoRepository(SmartStockDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Producto?> GetByIdAsync(int id)
    {
        var entity = await _context.Productos
            .Include(p => p.IdCategoriaNavigation)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (entity == null)
            return null;

        var domain = _mapper.Map<Producto>(entity);
        domain.NombreCategoria = entity.IdCategoriaNavigation?.Nombre ?? "(sin categoría)";
        return domain;
    }

    public async Task<IEnumerable<Producto>> GetAllByNegocioAsync(int idNegocio)
    {
        var lista = await _context.Productos
            .Include(p => p.IdCategoriaNavigation) // asegúrate que esté bien el nombre de navegación
            .Where(p => p.IdNegocio == idNegocio)
            .ToListAsync();

        return lista.Select(p =>
        {
            var domain = _mapper.Map<Producto>(p);
            domain.NombreCategoria = p.IdCategoriaNavigation?.Nombre ?? "(sin categoría)";
            return domain;
        }).ToList();
    }


    public async Task AddAsync(Producto producto)
    {
        var entity = _mapper.Map<Persistence.Models.Productos>(producto);
        await _context.Productos.AddAsync(entity);
        await _context.SaveChangesAsync();
        producto.Id = entity.Id;
    }

    public async Task PatchAsync(Producto producto)
    {
        var model = await _context.Productos.FindAsync(producto.Id);
        if (model == null) return;

        model.Nombre = producto.Nombre;
        model.Stock = producto.Stock;
        model.Umbral = producto.Umbral;
        model.PrecioVenta = producto.PrecioVenta;
        model.PrecioCompra = producto.PrecioCompra;
        model.PrecioDescuento = producto.PrecioDescuento;
        model.FechaIngreso = producto.FechaIngreso;
        model.Descripcion = producto.Descripcion;

        _context.Productos.Update(model);
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _context.Productos.FindAsync(id);
        if (model != null)
            _context.Productos.Remove(model);
    }
    
    public void Update(Producto producto)
    {
        var entity = _context.Productos.FirstOrDefault(p => p.Id == producto.Id);
        if (entity != null)
        {
            entity.Stock = producto.Stock;
            // Otros campos que desees actualizar
        }
    }
    public async Task<List<ProductoReportDto>> GetReporteProductosAsync(int idNegocio)
    {
        var productos = await _context.Productos
            .Include(p => p.IdCategoriaNavigation)
            .Where(p => p.IdNegocio == idNegocio)
            .ToListAsync();

        return _mapper.Map<List<ProductoReportDto>>(productos);
    }

}