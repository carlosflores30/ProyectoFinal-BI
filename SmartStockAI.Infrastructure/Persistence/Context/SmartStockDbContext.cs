using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Domain.Authentication.Entities;
using SmartStockAI.Domain.Roles.Entities;
using SmartStockAI.Infrastructure.Persistence.Models;

namespace SmartStockAI.Infrastructure.Persistence.Context;

public partial class SmartStockDbContext : DbContext
{
    public SmartStockDbContext()
    {
    }

    public SmartStockDbContext(DbContextOptions<SmartStockDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorias> Categorias { get; set; }

    public virtual DbSet<Clientes> Clientes { get; set; }

    public virtual DbSet<DetalleVenta> DetalleVenta { get; set; }

    public virtual DbSet<MovimientosInventario> MovimientosInventario { get; set; }

    public virtual DbSet<Models.Negocios> Negocios { get; set; }

    public virtual DbSet<Notificaciones> Notificaciones { get; set; }

    public virtual DbSet<Productos> Productos { get; set; }

    public virtual DbSet<Proveedores> Proveedores { get; set; }

    public virtual DbSet<RecomendacionesIa> RecomendacionesIa { get; set; }

    public virtual DbSet<Models.Roles> Roles { get; set; }
    public virtual DbSet<PasswordResetTokens> PasswordResetTokens { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    public virtual DbSet<Ventas> Ventas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //=> optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=smartstock_ai_db;Username=robertoflores;Password=302630");

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorias>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categorias_pkey");

            entity.ToTable("categorias");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IdNegocio).HasColumnName("id_negocio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdNegocioNavigation).WithMany(p => p.Categorias)
                .HasForeignKey(d => d.IdNegocio)
                .HasConstraintName("categorias_id_negocio_fkey");
        });

        modelBuilder.Entity<Clientes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clientes_pkey");

            entity.ToTable("clientes");

            entity.HasIndex(e => e.Dni, "clientes_dni_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .HasColumnName("correo");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .HasColumnName("dni");
            entity.Property(e => e.IdNegocio).HasColumnName("id_negocio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdNegocioNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdNegocio)
                .HasConstraintName("clientes_id_negocio_fkey");
        });

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("detalle_venta_pkey");

            entity.ToTable("detalle_venta");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.DescuentoAplicado)
                .HasPrecision(10, 2)
                .HasColumnName("descuento_aplicado");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("precio_unitario");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("detalle_venta_id_producto_fkey");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("detalle_venta_id_venta_fkey");
        });

        modelBuilder.Entity<MovimientosInventario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("movimientos_inventario_pkey");

            entity.ToTable("movimientos_inventario");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.FechaMovimiento)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_movimiento");
            entity.Property(e => e.IdNegocio).HasColumnName("id_negocio");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.Observacion).HasColumnName("observacion");
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(50)
                .HasColumnName("tipo_movimiento");

            entity.HasOne(d => d.IdNegocioNavigation).WithMany(p => p.MovimientosInventario)
                .HasForeignKey(d => d.IdNegocio)
                .HasConstraintName("movimientos_inventario_id_negocio_fkey");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.MovimientosInventario)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("movimientos_inventario_id_producto_fkey");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.MovimientosInventario)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("movimientos_inventario_id_usuario_fkey");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.MovimientosInventario)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("movimientos_inventario_id_venta_fkey");
        });

        modelBuilder.Entity<Models.Negocios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("negocios_pkey");

            entity.ToTable("negocios");

            entity.HasIndex(e => e.Ruc, "negocios_ruc_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(150)
                .HasColumnName("razon_social");
            entity.Property(e => e.Ruc)
                .HasMaxLength(20)
                .HasColumnName("ruc");

            entity.HasOne(d => d.IdUsuarioNavigation)
                .WithOne(p => p.Negocio)
                .HasForeignKey<Models.Negocios>(d => d.IdUsuario)
                .HasConstraintName("negocios_id_usuario_fkey");

        });

        modelBuilder.Entity<Notificaciones>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notificaciones_pkey");

            entity.ToTable("notificaciones");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha");

            entity.Property(e => e.IdNegocio).HasColumnName("id_negocio");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdMovimiento).HasColumnName("id_movimiento");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.Property(e => e.Leido)
                .HasDefaultValue(false)
                .HasColumnName("leido");

            entity.Property(e => e.Mensaje).HasColumnName("mensaje");
            entity.Property(e => e.Titulo).HasColumnName("titulo");

            entity.HasOne(d => d.IdNegocioNavigation).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.IdNegocio)
                .HasConstraintName("notificaciones_id_negocio_fkey");

            entity.HasOne(d => d.IdProductoNavigation)
                .WithMany()
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_notificaciones_producto");

            entity.HasOne(d => d.IdMovimientoNavigation)
                .WithMany()
                .HasForeignKey(d => d.IdMovimiento)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_notificaciones_movimiento");

            entity.HasOne(d => d.IdVentaNavigation)
                .WithMany()
                .HasForeignKey(d => d.IdVenta)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_notificaciones_venta");

            entity.HasOne(d => d.IdUsuarioNavigation)
                .WithMany()
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_notificaciones_usuario");
        });
        modelBuilder.Entity<Productos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("productos_pkey");

            entity.ToTable("productos");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CodProducto)
                .HasMaxLength(50)
                .HasColumnName("cod_producto");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.FechaIngreso).HasColumnName("fecha_ingreso");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.IdNegocio).HasColumnName("id_negocio");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.PrecioCompra)
                .HasPrecision(10, 2)
                .HasColumnName("precio_compra");
            entity.Property(e => e.PrecioDescuento)
                .HasPrecision(10, 2)
                .HasColumnName("precio_descuento");
            entity.Property(e => e.PrecioVenta)
                .HasPrecision(10, 2)
                .HasColumnName("precio_venta");
            entity.Property(e => e.Stock).HasColumnName("stock");
            entity.Property(e => e.Umbral).HasColumnName("umbral");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("productos_id_categoria_fkey");

            entity.HasOne(d => d.IdNegocioNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdNegocio)
                .HasConstraintName("productos_id_negocio_fkey");
        });
        
        modelBuilder.Entity<PasswordResetTokens>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("password_reset_tokens_pkey");

            entity.ToTable("password_reset_tokens");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Expiration).HasColumnName("expiration");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Token)
                .HasMaxLength(100)
                .HasColumnName("token");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.PasswordResetTokens)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("password_reset_tokens_id_usuario_fkey");
        });

        modelBuilder.Entity<Proveedores>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("proveedores_pkey");

            entity.ToTable("proveedores");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.IdNegocio).HasColumnName("id_negocio");
            entity.Property(e => e.NombreEmpresa)
                .HasMaxLength(150)
                .HasColumnName("nombre_empresa");
            entity.Property(e => e.Ruc)
                .HasMaxLength(20)
                .HasColumnName("ruc");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdNegocioNavigation).WithMany(p => p.Proveedores)
                .HasForeignKey(d => d.IdNegocio)
                .HasConstraintName("proveedores_id_negocio_fkey");
        });

        modelBuilder.Entity<RecomendacionesIa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("recomendaciones_ia_pkey");

            entity.ToTable("recomendaciones_ia");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha");
            entity.Property(e => e.IdNegocio).HasColumnName("id_negocio");
            entity.Property(e => e.Prompt).HasColumnName("prompt");
            entity.Property(e => e.Respuesta).HasColumnName("respuesta");

            entity.HasOne(d => d.IdNegocioNavigation).WithMany(p => p.RecomendacionesIa)
                .HasForeignKey(d => d.IdNegocio)
                .HasConstraintName("recomendaciones_ia_id_negocio_fkey");
        });

        modelBuilder.Entity<Models.Roles>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "usuarios_correo_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .HasColumnName("apellido");
            entity.Property(e => e.BloqueadoHasta).HasColumnName("bloqueado_hasta");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .HasColumnName("correo");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.IntentosFallidos)
                .HasDefaultValue(0)
                .HasColumnName("intentos_fallidos");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
            entity.Property(e => e.NegocioId).HasColumnName("negocio_id");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("usuarios_id_rol_fkey");
            entity.HasOne(u => u.Negocio)
                .WithOne(n => n.IdUsuarioNavigation)
                .HasForeignKey<Models.Negocios>(n => n.IdUsuario)
                .HasConstraintName("negocios_id_usuario_fkey");
            entity.HasOne(u => u.NegocioNavigation)
                .WithOne()
                .HasForeignKey<Usuarios>(u => u.NegocioId)
                .HasConstraintName("usuarios_negocio_id_fkey")
                .OnDelete(DeleteBehavior.SetNull);

        });

        modelBuilder.Entity<Ventas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ventas_pkey");

            entity.ToTable("ventas");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.FechaVenta)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("fecha_venta");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdNegocio).HasColumnName("id_negocio");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .HasColumnName("metodo_pago");
            entity.Property(e => e.TotalVenta)
                .HasPrecision(10, 2)
                .HasColumnName("total_venta");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Ventas)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("ventas_id_cliente_fkey");

            entity.HasOne(d => d.IdNegocioNavigation).WithMany(p => p.Ventas)
                .HasForeignKey(d => d.IdNegocio)
                .HasConstraintName("ventas_id_negocio_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
