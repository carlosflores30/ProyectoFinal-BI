using System.Text;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartStockAI.Application.Interfaces.Authentication;
using SmartStockAI.Application.Interfaces.Notifications;
using SmartStockAI.Application.UsesCases.Authentication.Commands;
using SmartStockAI.Domain.Authentication.Interfaces;
using SmartStockAI.Domain.Negocios.Interfaces;
using SmartStockAI.Domain.Roles.Interfaces;
using SmartStockAI.Domain.UnitOfWork.Interfaces;
using SmartStockAI.Infrastructure.Authentication.Repositories;
using SmartStockAI.Infrastructure.Authentication.Security;
using SmartStockAI.Infrastructure.Authentication.Services;
using SmartStockAI.Infrastructure.Configuration;
using SmartStockAI.Infrastructure.Mappers;
using SmartStockAI.Infrastructure.Negocios.Repositories;
using SmartStockAI.Infrastructure.Persistence.Context;
using SmartStockAI.Infrastructure.Roles.Repositories;
using SmartStockAI.Infrastructure.UnitOfWork;

namespace SmartStockAI.Api.Configuration;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddHttpContextAccessor();
        services.AddHangfire(config =>
            config.UsePostgreSqlStorage(configuration.GetConnectionString(
                "DefaultConnection")));
        services.AddHangfireServer();
        services.AddEndpointsApiExplorer();
        services.AddInfrastructure(configuration);
        services.AddDbContext<SmartStockDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly);
        });
        
        var jwtSettings = configuration.GetSection("Jwt");
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };
            });

        // 8. Swagger
        services.AddControllers();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartStockAI API", Version = "v1" });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Introduce solo el token. El prefijo 'Bearer' será agregado automáticamente."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });
        
        return services;
    }
}
