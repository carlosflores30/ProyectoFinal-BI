using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Domain.Authentication.Entities;
using SmartStockAI.Domain.Authentication.Interfaces;
using SmartStockAI.Infrastructure.Persistence.Context;
using SmartStockAI.Infrastructure.Persistence.Models;

namespace SmartStockAI.Infrastructure.Authentication.Repositories;

public class PasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly SmartStockDbContext _context;
    private readonly IMapper _mapper;

    public PasswordResetTokenRepository(SmartStockDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PasswordResetToken?> GetByTokenAsync(string token)
    {
        var model = await _context.PasswordResetTokens
            .FirstOrDefaultAsync(x => x.Token == token);

        return model == null ? null : _mapper.Map<PasswordResetToken>(model);
    }

    public async Task AddAsync(PasswordResetToken token)
    {
        var entity = _mapper.Map<PasswordResetTokens>(token);
        await _context.PasswordResetTokens.AddAsync(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.PasswordResetTokens.FindAsync(id);
        if (entity != null)
            _context.PasswordResetTokens.Remove(entity);
    }
}