using AutoMapper;
using SmartStockAI.Application.DTOs.Authentication;
using SmartStockAI.Application.DTOs.Categories;
using SmartStockAI.Application.DTOs.Clients;
using SmartStockAI.Application.DTOs.Inventory;
using SmartStockAI.Application.DTOs.Negocios;
using SmartStockAI.Application.DTOs.Notifications;
using SmartStockAI.Application.DTOs.Products;
using SmartStockAI.Application.DTOs.Providers;
using SmartStockAI.Application.DTOs.Reports;
using SmartStockAI.Application.DTOs.Roles;
using SmartStockAI.Application.DTOs.Sales;
using SmartStockAI.Domain.Authentication.Entities;
using SmartStockAI.Domain.Categories.Entities;
using SmartStockAI.Domain.Clients.Entities;
using SmartStockAI.Domain.Inventory.Entities;
using SmartStockAI.Domain.Negocios.Entities;
using SmartStockAI.Domain.Notifications.Entities;
using SmartStockAI.Domain.Products.Entities;
using SmartStockAI.Domain.Providers.Entities;
using SmartStockAI.Domain.Sales.Entities;
using SmartStockAI.Infrastructure.Persistence.Models;
using DomainRole = SmartStockAI.Domain.Roles.Entities.Role;
using EfRole = SmartStockAI.Infrastructure.Persistence.Models.Roles;

namespace SmartStockAI.Infrastructure.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Usuarios, Usuario>()
            .ForMember(dest => dest.RoleName,
                opt => opt.MapFrom(
                    src => src.IdRolNavigation != null ? src.IdRolNavigation.Nombre : null))
            .ForMember(dest => dest.NegocioId, opt => opt.MapFrom(src => src.NegocioId));
        CreateMap<Usuario, Usuarios>();
        CreateMap<Usuario, LoginResponse>()
            .ForMember(dest => dest.NegocioId, 
                opt => opt.MapFrom(src => src.NegocioId));
        CreateMap<LoginRequest, Usuario>();

          //Roles
        CreateMap<EfRole, DomainRole>().ReverseMap();
        CreateMap<EfRole, DomainRole>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nombre))
            .ReverseMap()
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Name));

        CreateMap<DomainRole, RoleDto>().ReverseMap();
        //Recuperar Contraseña
        CreateMap<PasswordResetTokens, PasswordResetToken>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.IdUsuario))
            .ReverseMap()
            .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => src.UserId));
        //Negocios
        CreateMap<Persistence.Models.Negocios, Negocio>()
            .ForMember(dest => dest.Usuario, opt => opt.Ignore()); 
        CreateMap<Negocio, Persistence.Models.Negocios>()
            .ForMember(dest => dest.IdUsuarioNavigation, opt => opt.Ignore());
        CreateMap<Negocio, NegocioDto>().ReverseMap();
        CreateMap<CrearNegocioDto, Negocio>();
        
        CreateMap<Usuario, UsuarioDto>()
            .ForMember(dest => dest.Negocio, opt => opt.MapFrom(src => src.Negocio));
        
        CreateMap<Negocio, NegocioMiniDto>();
        
        //Productos
        CreateMap<Persistence.Models.Productos, Producto>().ReverseMap(); 
        CreateMap<Producto, ProductoDto>().ReverseMap();                 
        CreateMap<CreateProductoDto, Producto>();      
        
        // Categoría
        CreateMap<Persistence.Models.Categorias, Categoria>()
            .ForMember(dest => dest.IdNegocio, opt => opt.MapFrom(src => src.IdNegocio ?? 0));
        CreateMap<Categoria, Persistence.Models.Categorias>()
            .ForMember(dest => dest.IdNegocio, opt => opt.MapFrom(src => (int?)src.IdNegocio));
        CreateMap<Categoria, CategoriaDto>().ReverseMap();
        CreateMap<CreateCategoriaDto, Categoria>();

// Cliente
        CreateMap<Persistence.Models.Clientes, Cliente>().ReverseMap();
        CreateMap<Cliente, ClienteDto>().ReverseMap();
        CreateMap<Cliente, CreateClienteDto>().ReverseMap();

// Proveedor
        CreateMap<Persistence.Models.Proveedores, Proveedor>().ReverseMap();
        CreateMap<Proveedor, ProveedorDto>().ReverseMap();
        CreateMap<Proveedor, CreateProveedorDto>().ReverseMap();

// Movimiento Inventario
        CreateMap<MovimientoInventario, InventoryDto>().ReverseMap();
        CreateMap<MovimientosInventario, MovimientoInventario>()
            .ForMember(dest => dest.Producto, opt => opt.MapFrom(src => src.IdProductoNavigation))
            .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => src.IdUsuarioNavigation.Id)) // 👈 esto
            .ForMember(dest => dest.IdVenta, opt => opt.MapFrom(src => src.IdVentaNavigation != null ? src.IdVentaNavigation.Id : (int?)null));

        CreateMap<MovimientoInventario, MovimientosInventario>();

// Venta

// Venta EF → Entidad de Dominio
        CreateMap<Persistence.Models.Ventas, Venta>()
            .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.IdClienteNavigation))
            .ForMember(dest => dest.DetalleVenta, opt => opt.MapFrom(src => src.DetalleVenta));

        // Detalle EF → Entidad de Dominio
        CreateMap<Persistence.Models.DetalleVenta, DetalleDeVenta>()
            .ForMember(dest => dest.Producto, opt => opt.MapFrom(src => src.IdProductoNavigation));

        // Venta → DTO
        CreateMap<Venta, SaleDto>()
            .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nombre : null))
            .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.DetalleVenta));

        // Detalle → DTO
        CreateMap<DetalleDeVenta, DetailSaleDto>()
            .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.Producto!.Nombre));

        // DTO → Dominio
        CreateMap<CreateSaleDto, Venta>();
        CreateMap<CreateDetailSaleDto, DetalleDeVenta>();
        CreateMap<DetailSaleDto, DetalleDeVenta>();
        // Dominio → EF
        CreateMap<Venta, Persistence.Models.Ventas>()
            .ForMember(dest => dest.DetalleVenta, opt => opt.MapFrom(src => src.DetalleVenta))
            .ForMember(dest => dest.IdClienteNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.IdNegocioNavigation, opt => opt.Ignore());

// Dominio → EF para Detalle
        CreateMap<DetalleDeVenta, Persistence.Models.DetalleVenta>()
            .ForMember(dest => dest.IdProductoNavigation, opt => opt.Ignore())
            .ForMember(dest => dest.IdVentaNavigation, opt => opt.Ignore());


        // (Opcional) Para devolver directamente desde EF (si es necesario)
        CreateMap<Persistence.Models.Ventas, SaleDto>()
            .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.IdClienteNavigation.Nombre))
            .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.DetalleVenta));

        CreateMap<Persistence.Models.DetalleVenta, DetailSaleDto>()
            .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.IdProductoNavigation.Nombre));
         
        // Reports
        CreateMap<Productos, ProductoReportDto>()
            .ForMember(dest => dest.Categoria,
                opt => opt.MapFrom(src => src.IdCategoriaNavigation != null ? src.IdCategoriaNavigation.Nombre : ""))
            .ForMember(dest => dest.FechaIngreso,
                opt => opt.MapFrom(src => src.FechaIngreso.HasValue 
                    ? src.FechaIngreso.Value.ToDateTime(TimeOnly.MinValue)
                    : DateTime.MinValue));
        
        // Reporte de Clientes
        // Mapeo para reportes de clientes (desde entidad EF directamente al DTO de reporte)
        CreateMap<Cliente, ClienteReportDto>();
        CreateMap<Clientes, ClienteReportDto>()
            .ForMember(dest => dest.Documento, opt => opt.MapFrom(src => src.Dni));
        
        
        CreateMap<Ventas, VentaReportDto>()
            .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.IdClienteNavigation.Nombre))
            .ForMember(dest => dest.MetodoPago, opt => opt.MapFrom(src => src.MetodoPago))
            .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.TotalVenta));
        
        
        CreateMap<DetalleVenta, DetalleVentaReportDto>()
            .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => src.IdVentaNavigation.IdClienteNavigation.Nombre))
            .ForMember(dest => dest.Producto, opt => opt.MapFrom(src => src.IdProductoNavigation.Nombre))
            .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.IdVentaNavigation.FechaVenta))
            .ForMember(dest => dest.TotalItem, opt => opt.MapFrom(src => 
                (src.PrecioUnitario - src.DescuentoAplicado) * src.Cantidad));
        
        
        CreateMap<Notificaciones, Notificacion>();  
        CreateMap<Notificacion, Notificaciones>();  
        CreateMap<Notificacion, NotificacionDto>();
        CreateMap<Notificaciones, NotificacionDto>();
        CreateMap<RegistrarNotificacionDto, Notificacion>();
    }
}