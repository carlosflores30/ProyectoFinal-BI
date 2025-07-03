using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Domain.Providers.Entities;
using SmartStockAI.Domain.Providers.Interfaces;
using SmartStockAI.Infrastructure.Persistence.Context;

namespace SmartStockAI.Infrastructure.Providers.Repositories;

public class ProveedorRepository : IProveedorRepository
{
    private readonly SmartStockDbContext _context;
    private readonly IMapper _mapper;

    public ProveedorRepository(SmartStockDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Proveedor?> GetByIdAsync(int id)
    {
        var model = await _context.Proveedores.FindAsync(id);
        return model == null ? null : _mapper.Map<Proveedor>(model);
    }

    public async Task<IEnumerable<Proveedor>> GetAllByNegocioAsync(int idNegocio)
    {
        var lista = await _context.Proveedores
            .Where(p => p.IdNegocio == idNegocio)
            .ToListAsync();

        return _mapper.Map<IEnumerable<Proveedor>>(lista);
    }

    public async Task AddAsync(Proveedor proveedor)
    {
        var model = _mapper.Map<Persistence.Models.Proveedores>(proveedor);
        await _context.Proveedores.AddAsync(model);
        await _context.SaveChangesAsync();
        proveedor.Id = model.Id;
    }

    public async Task PatchAsync(Proveedor proveedor)
    {
        var model = await _context.Proveedores.FindAsync(proveedor.Id);
        if (model == null) return;

        if (!string.IsNullOrWhiteSpace(proveedor.NombreEmpresa))
            model.NombreEmpresa = proveedor.NombreEmpresa;
        if (!string.IsNullOrWhiteSpace(proveedor.Ruc))
            model.Ruc = proveedor.Ruc;
        if (!string.IsNullOrWhiteSpace(proveedor.Direccion))
            model.Direccion = proveedor.Direccion;
        if (!string.IsNullOrWhiteSpace(proveedor.Telefono))
            model.Telefono = proveedor.Telefono;
        if (!string.IsNullOrWhiteSpace(proveedor.Correo))
            model.Correo = proveedor.Correo;

        _context.Proveedores.Update(model);
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _context.Proveedores.FindAsync(id);
        if (model != null)
            _context.Proveedores.Remove(model);
    }
}