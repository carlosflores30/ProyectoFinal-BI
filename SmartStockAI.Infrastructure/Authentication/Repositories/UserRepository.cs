using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Domain.Authentication.Entities;
using SmartStockAI.Domain.Authentication.Interfaces;
using SmartStockAI.Infrastructure.Persistence.Context;
using SmartStockAI.Infrastructure.Persistence.Models;

//using SmartStockAI.Infrastructure.Persistence.Context;
//using SmartStockAI.Infrastructure.Persistence.Models;

namespace SmartStockAI.Infrastructure.Authentication.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SmartStockDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(SmartStockDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        var entity = await _context.Usuarios
            .Include(u => u.IdRolNavigation)
            .FirstOrDefaultAsync(u => u.Correo == email);

        return entity == null ? null : _mapper.Map<Usuario>(entity);
    }
    
    internal async Task<Usuarios?> GetEfUserByEmailAsync(string email)
    {
        return await _context.Usuarios
            .Include(u => u.IdRolNavigation)
            .FirstOrDefaultAsync(u => u.Correo == email);
    }
    public async Task UpdatePasswordAsync(int userId, string hashedPassword)
    {
        var entity = await _context.Usuarios.FindAsync(userId);
        if (entity == null)
            throw new InvalidOperationException("El usuario no existe.");

        entity.Contrasena = hashedPassword;
    }


    public async Task<Usuario?> GetByIdAsync(int id)
    {
        var model = await _context.Usuarios.FindAsync(id);
        return model == null ? null : _mapper.Map<Usuario>(model);
    }

    public async Task AddAsync(Usuario user)
    {
        var model = _mapper.Map<Usuarios>(user);
        await _context.Usuarios.AddAsync(model);
        await _context.SaveChangesAsync();
        user.Id = model.Id;
    }
    
    public async Task<Usuario> GetByIdWithNegocioAsync(int id)
    {
        var model = await _context.Usuarios
            .Include(u => u.Negocio)
            .FirstOrDefaultAsync(u => u.Id == id);

        return _mapper.Map<Usuario>(model); // Entidad del dominio
    }

}