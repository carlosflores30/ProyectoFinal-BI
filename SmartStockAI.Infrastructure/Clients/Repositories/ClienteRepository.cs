using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Application.DTOs.Reports;
using SmartStockAI.Application.Interfaces.Reports;
using SmartStockAI.Domain.Clients.Entities;
using SmartStockAI.Domain.Clients.Interfaces;
using SmartStockAI.Infrastructure.Persistence.Context;

namespace SmartStockAI.Infrastructure.Clients.Repositories;

public class ClienteRepository : IClienteRepository , IClienteReportService
{
    private readonly SmartStockDbContext _context;
    private readonly IMapper _mapper;

    public ClienteRepository(SmartStockDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ClienteReportDto>> GetReporteClientesAsync(int idNegocio)
    {
        var clientes = await _context.Clientes
            .Where(c => c.IdNegocio == idNegocio)
            .ToListAsync();

        return _mapper.Map<List<ClienteReportDto>>(clientes);
    }
    public async Task<Cliente?> GetByIdAsync(int id)
    {
        var model = await _context.Clientes.FindAsync(id);
        return model == null ? null : _mapper.Map<Cliente>(model);
    }

    public async Task<IEnumerable<Cliente>> GetAllByNegocioAsync(int idNegocio)
    {
        var lista = await _context.Clientes
            .Where(c => c.IdNegocio == idNegocio)
            .ToListAsync();

        return _mapper.Map<IEnumerable<Cliente>>(lista);
    }

    public async Task AddAsync(Cliente cliente)
    {
        var model = _mapper.Map<Persistence.Models.Clientes>(cliente);
        await _context.Clientes.AddAsync(model);
        await _context.SaveChangesAsync();
        cliente.Id = model.Id;
    }

    public async Task PatchAsync(Cliente cliente)
    {
        var model = await _context.Clientes.FindAsync(cliente.Id);
        if (model == null) return;

        if (!string.IsNullOrWhiteSpace(cliente.Nombre)) model.Nombre = cliente.Nombre;
        if (!string.IsNullOrWhiteSpace(cliente.Correo)) model.Correo = cliente.Correo;
        if (!string.IsNullOrWhiteSpace(cliente.Telefono)) model.Telefono = cliente.Telefono;
        if (!string.IsNullOrWhiteSpace(cliente.Direccion)) model.Direccion = cliente.Direccion;

        _context.Clientes.Update(model);
    }

    public async Task DeleteAsync(int id)
    {
        var model = await _context.Clientes.FindAsync(id);
        if (model != null)
            _context.Clientes.Remove(model);
    }
}