using AutoMapper;
using MediatR;
using SmartStockAI.Application.DTOs.Providers;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Providers.Commands;

public record PatchProveedorCommand(int IdProveedor, int IdNegocio, PatchProveedorDto Dto) : IRequest<bool>;

public class PatchProveedorCommandHandler : IRequestHandler<PatchProveedorCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PatchProveedorCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(PatchProveedorCommand request, CancellationToken cancellationToken)
    {
        var proveedor = await _unitOfWork.ProveedorRepository.GetByIdAsync(request.IdProveedor);

        if (proveedor == null || proveedor.IdNegocio != request.IdNegocio)
            return false;

        // Actualizar valores si no son nulos
        if (!string.IsNullOrWhiteSpace(request.Dto.NombreEmpresa))
            proveedor.NombreEmpresa = request.Dto.NombreEmpresa;

        if (!string.IsNullOrWhiteSpace(request.Dto.Ruc))
            proveedor.Ruc = request.Dto.Ruc;

        if (!string.IsNullOrWhiteSpace(request.Dto.Direccion))
            proveedor.Direccion = request.Dto.Direccion;

        if (!string.IsNullOrWhiteSpace(request.Dto.Telefono))
            proveedor.Telefono = request.Dto.Telefono;

        if (!string.IsNullOrWhiteSpace(request.Dto.Correo))
            proveedor.Correo = request.Dto.Correo;

        await _unitOfWork.ProveedorRepository.PatchAsync(proveedor);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}