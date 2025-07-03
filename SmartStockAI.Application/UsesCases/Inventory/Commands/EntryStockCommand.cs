using Hangfire;
using MediatR;
using SmartStockAI.Application.DTOs.EntriesStock;
using SmartStockAI.Application.Interfaces.Notifications;
using SmartStockAI.Domain.Inventory.Entities;
using SmartStockAI.Domain.UnitOfWork.Interfaces;

namespace SmartStockAI.Application.UsesCases.Inventory.Commands;

public record EntryStockCommand(EntryStockDto Dto, int IdNegocio, int IdUsuario) : IRequest<bool>;

public class EntryStockCommandHandler(IUnitOfWork _unitOfWork) : IRequestHandler<EntryStockCommand, bool>
{
    public async Task<bool> Handle(EntryStockCommand request, CancellationToken cancellationToken)
    {
        var producto = await _unitOfWork.ProductosRepository.GetByIdAsync(request.Dto.IdProducto);

        if (producto == null || producto.IdNegocio != request.IdNegocio)
            return false;

        producto.Stock += request.Dto.Cantidad;
        _unitOfWork.ProductosRepository.Update(producto);

        var movimiento = new MovimientoInventario
        {
            IdProducto = request.Dto.IdProducto,
            TipoMovimiento = "Entrada",
            Cantidad = request.Dto.Cantidad,
            FechaMovimiento = DateTime.UtcNow,
            Observacion = request.Dto.Observacion,
            IdUsuario = request.IdUsuario,
            IdNegocio = request.IdNegocio
        };

        var idmovimiento = await _unitOfWork.MovimientoInventarioRepository.AddAsync(movimiento);
        
        Console.WriteLine($"ID generado: {movimiento.Id}");
        BackgroundJob.Enqueue<IAlertaSistemaService>(s =>
            s.GenerarAlertaPorMovimientoAsync(idmovimiento));
        return true;
    }
}