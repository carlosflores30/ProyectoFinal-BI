using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Domain.Negocios.Entities;
using SmartStockAI.Domain.Negocios.Interfaces;
using SmartStockAI.Infrastructure.Persistence.Context;

namespace SmartStockAI.Infrastructure.Negocios.Repositories;

public class NegocioRepository : INegocioRepository
{
    private readonly SmartStockDbContext _context;
    private readonly IMapper _mapper;

    public NegocioRepository(SmartStockDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;        
    }

    public async Task<Negocio?> GetByIdAsync(int id)
    {
        var model = await _context.Negocios.FindAsync(id);
        return model == null ? null : _mapper.Map<Negocio>(model);
    }

    public async Task<IEnumerable<Negocio>> GetAllByUsuarioAsync(int idUsuario)
    {
        var negocios = await _context.Negocios
            .Where(n => n.IdUsuario == idUsuario)
            .ToListAsync();

        return _mapper.Map<IEnumerable<Negocio>>(negocios);
    }

    public async Task AddAsync(Negocio negocio)
    {
        var model = _mapper.Map<Persistence.Models.Negocios>(negocio);
        await _context.Negocios.AddAsync(model);
        await _context.SaveChangesAsync();
        negocio.Id = model.Id;
    }

    public void Update(Negocio negocio)
    {
        var model = _mapper.Map<Persistence.Models.Negocios>(negocio);
        _context.Negocios.Update(model);
    }

    public void Delete(Negocio negocio)
    {
        var model = _mapper.Map<Persistence.Models.Negocios>(negocio);
        _context.Negocios.Remove(model);
    }
    public async Task<IEnumerable<Negocio>> ObtenerTodosAsync()
    {
        var lista = await _context.Negocios.ToListAsync();
        return _mapper.Map<List<Negocio>>(lista);
    }
}