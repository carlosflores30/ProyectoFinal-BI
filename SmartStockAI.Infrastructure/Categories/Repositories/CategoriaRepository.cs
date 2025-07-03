using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Domain.Categories.Entities;
using SmartStockAI.Domain.Categories.Interfaces;
using SmartStockAI.Infrastructure.Persistence.Context;
using SmartStockAI.Infrastructure.Persistence.Models;

namespace SmartStockAI.Infrastructure.Categories.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly SmartStockDbContext _context;
    private readonly IMapper _mapper;

    public CategoriaRepository(SmartStockDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Categoria>> GetAllByNegocioAsync(int idNegocio)
    {
        var lista = await _context.Categorias
            .Where(c => c.IdNegocio == idNegocio)
            .ToListAsync();

        return _mapper.Map<List<Categoria>>(lista);
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        var model = await _context.Categorias.FindAsync(id);
        return model == null ? null : _mapper.Map<Categoria>(model);
    }

    public async Task AddAsync(Categoria categoria)
    {
        var model = _mapper.Map<Categorias>(categoria);
        await _context.Categorias.AddAsync(model);
        await _context.SaveChangesAsync();
        categoria.Id = model.Id;
    }
    
    public async Task PatchAsync(Categoria categoria)
    {
        var model = await _context.Categorias.FindAsync(categoria.Id);
        if (model == null) return;

        if (!string.IsNullOrWhiteSpace(categoria.Nombre))
            model.Nombre = categoria.Nombre;

        if (categoria.IdNegocio > 0)
            model.IdNegocio = categoria.IdNegocio;

        _context.Categorias.Update(model);
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _context.Categorias.FindAsync(id);
        if (model != null)
            _context.Categorias.Remove(model);
    }
}