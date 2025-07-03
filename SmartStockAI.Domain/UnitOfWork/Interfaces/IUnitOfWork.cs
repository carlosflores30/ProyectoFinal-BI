using SmartStockAI.Domain.Authentication.Interfaces;
using SmartStockAI.Domain.Categories.Interfaces;
using SmartStockAI.Domain.Clients.Interfaces;
using SmartStockAI.Domain.Inventory.Interfaces;
using SmartStockAI.Domain.Negocios.Interfaces;
using SmartStockAI.Domain.Notifications.Interfaces;
using SmartStockAI.Domain.Roles.Interfaces;
using SmartStockAI.Domain.Products.Interfaces;
using SmartStockAI.Domain.Providers.Interfaces;
using SmartStockAI.Domain.Sales.Interfaces;


namespace SmartStockAI.Domain.UnitOfWork.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IRoleRepository RolesRepository { get; }
    INegocioRepository NegociosRepository { get; }
    IMovimientoInventarioRepository MovimientoInventarioRepository { get; }
    ISaleRepository SaleRepository { get; }
    IProductoRepository ProductosRepository { get; }
    ICategoriaRepository CategoriaRepository { get; }
    IClienteRepository ClienteRepository { get; }
    IProveedorRepository ProveedorRepository { get; }
    IDetailSaleRepository DetailSaleRepository { get; }
    INotificacionRepository NotificacionRepository { get; }
    Task SaveChangesAsync();
}