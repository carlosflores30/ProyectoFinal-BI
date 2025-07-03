using AutoMapper;
using Microsoft.EntityFrameworkCore;
using DomainRole = SmartStockAI.Domain.Roles.Entities.Role;
using EfRole = SmartStockAI.Infrastructure.Persistence.Models.Roles;
using SmartStockAI.Domain.Roles.Interfaces;
using SmartStockAI.Infrastructure.Persistence.Context;

namespace SmartStockAI.Infrastructure.Roles.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly SmartStockDbContext _context;
    private readonly IMapper _mapper;

    public RoleRepository(SmartStockDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DomainRole>> GetAllAsync()
    {
        var roles = await _context.Roles.ToListAsync();
        return _mapper.Map<List<DomainRole>>(roles);
    }

    public async Task<DomainRole?> GetByIdAsync(int id)
    {
        var role = await _context.Roles.FindAsync(id);
        return _mapper.Map<DomainRole>(role);
    }
    
    public async Task AddAsync(DomainRole role)
    {
        var model = _mapper.Map<EfRole>(role);
        await _context.Roles.AddAsync(model);
        await _context.SaveChangesAsync();
        
        role.Id = model.Id;
    }
    
    public async Task DeleteAsync(int id)
    {
        var model = await _context.Roles.FindAsync(id);
        if (model != null)
        {
            _context.Roles.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}