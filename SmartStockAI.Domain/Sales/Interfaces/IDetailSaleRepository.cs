using SmartStockAI.Domain.Sales.Entities;

namespace SmartStockAI.Domain.Sales.Interfaces;

public interface IDetailSaleRepository
{
    Task AddRangeAsync(IEnumerable<DetalleDeVenta> detalles);
    Task<IEnumerable<DetalleDeVenta>> GetByVentaIdAsync(int idVenta);
    Task DeleteByVentaIdAsync(int idVenta);
}