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
using SmartStockAI.Domain.UnitOfWork.Interfaces;
using SmartStockAI.Infrastructure.Persistence.Context;

namespace SmartStockAI.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly SmartStockDbContext _context;

    public IUserRepository Users { get; }
    public IRoleRepository RolesRepository { get; }
    public INegocioRepository NegociosRepository { get; }
    public IProductoRepository ProductosRepository { get; }
    public ICategoriaRepository CategoriaRepository { get; }
    public IClienteRepository ClienteRepository { get; }
    public IProveedorRepository ProveedorRepository { get; }
    public IMovimientoInventarioRepository MovimientoInventarioRepository { get; }
    public ISaleRepository SaleRepository { get; }
    public IDetailSaleRepository DetailSaleRepository { get; }
    public INotificacionRepository NotificacionRepository { get; }
    public UnitOfWork(
        SmartStockDbContext context,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        INegocioRepository negocioRepository,
        IProductoRepository productoRepository,
        ICategoriaRepository categoriaRepository,
        IProveedorRepository proveedorRepository,
        IClienteRepository clienteRepository,
        IMovimientoInventarioRepository movimientoInventarioRepository,
        ISaleRepository saleRepository,
        IDetailSaleRepository detailSaleRepository,
        INotificacionRepository notificacionRepository)
    {
        _context = context;
        Users = userRepository;
        RolesRepository = roleRepository;
        NegociosRepository = negocioRepository;
        ProductosRepository = productoRepository;
        CategoriaRepository = categoriaRepository;
        ClienteRepository = clienteRepository;
        ProveedorRepository = proveedorRepository;
        MovimientoInventarioRepository = movimientoInventarioRepository;
        SaleRepository = saleRepository;
        DetailSaleRepository = detailSaleRepository;    
        NotificacionRepository = notificacionRepository;
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}