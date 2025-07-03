using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Application.Interfaces.Notifications;
using SmartStockAI.Application.Interfaces.Reports;
using SmartStockAI.Domain.Authentication.Interfaces;
using SmartStockAI.Domain.Categories.Interfaces;
using SmartStockAI.Domain.Clients.Interfaces;
using SmartStockAI.Domain.Inventory.Interfaces;
using SmartStockAI.Domain.Negocios.Interfaces;
using SmartStockAI.Domain.Notifications.Interfaces;
using SmartStockAI.Domain.Products.Interfaces;
using SmartStockAI.Domain.Providers.Interfaces;
using SmartStockAI.Domain.Roles.Interfaces;
using SmartStockAI.Domain.Sales.Interfaces;
using SmartStockAI.Domain.UnitOfWork.Interfaces;
using SmartStockAI.Infrastructure.Authentication.Repositories;
using SmartStockAI.Infrastructure.Authentication.Security;
using SmartStockAI.Infrastructure.Authentication.Services;
using SmartStockAI.Infrastructure.Categories.Repositories;
using SmartStockAI.Infrastructure.Clients.Repositories;
using SmartStockAI.Infrastructure.Inventory.Repositories;
using SmartStockAI.Infrastructure.Mappers;
using SmartStockAI.Infrastructure.Negocios.Repositories;
using SmartStockAI.Infrastructure.Notifications.Repositories;
using SmartStockAI.Infrastructure.Notifications.Services;
using SmartStockAI.Infrastructure.Persistence.Context;
using SmartStockAI.Infrastructure.Products.Repositories;
using SmartStockAI.Infrastructure.Providers.Repositories;
using SmartStockAI.Infrastructure.Reports.Interfaces;
using SmartStockAI.Infrastructure.Reports.Services;
using SmartStockAI.Infrastructure.Roles.Repositories;
using SmartStockAI.Infrastructure.Sales.Repositories;

//using SmartStockAI.Infrastructure.Persistence.Context;

namespace SmartStockAI.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        
        
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
        services.AddScoped<IFrontendUrlProvider, FrontendUrlProvider>();
        services.AddScoped<IProductoRepository, ProductoRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<INegocioRepository, NegocioRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IProveedorRepository, ProveedorRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IDetailSaleRepository, DetailSaleRepository>();
        services.AddScoped<IMovimientoInventarioRepository, MovimientoInventarioRepository>();
        services.AddScoped<IMovimientoInventarioReportService, MovimientoInventarioRepository>();
        services.AddScoped<IReporteExcelService, ReporteExcelService>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IProductoReportService, ProductoRepository>();
        services.AddScoped<IClienteReportService, ClienteRepository>();
        services.AddScoped<IVentaReportService, SaleRepository>();
        services.AddScoped<IDetalleVentaReportService, DetailSaleRepository>();
        services.AddScoped<INotificacionRepository, NotificacionRepository>();
        services.AddScoped<IAlertaSistemaService, AlertaSistemaService>();


        return services;
    }
}