using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartStockAI.Api.Configuration;
using SmartStockAI.Api.Middleware;
using SmartStockAI.Application.Interfaces.Notifications;
using SmartStockAI.Application.UsesCases.Authentication.Commands;
using SmartStockAI.Infrastructure;
using SmartStockAI.Infrastructure.Mappers;
using SmartStockAI.Infrastructure.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// CORS Policy
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://frontend2-f5en.onrender.com") // ← frontend correcto
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Render requiere configurar el puerto
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

// Swagger y servicios de infraestructura
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProjectServices(builder.Configuration);

var app = builder.Build();

// ORDEN CORRECTO DE MIDDLEWARES
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins); // 🔥 CORS aquí

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<IdleTimeoutMiddleware>();
app.UseMiddleware<AuthenticationValidationMiddleware>();

app.UseHttpsRedirection();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartStockAI API v1");
    c.RoutePrefix = string.Empty;
});

// Hangfire
app.UseHangfireDashboard("/hangfire");

// Rutas
app.MapControllers();

// Jobs programados
RecurringJob.AddOrUpdate<IAlertaSistemaService>(
    "verificar-stock-bajo",
    s => s.VerificarProductosConStockBajoAsync(),
    Cron.Hourly);

RecurringJob.AddOrUpdate<IAlertaSistemaService>(
    "verificar-baja-rotacion",
    s => s.VerificarProductosConBajaRotacionAsync(),
    Cron.Daily);

app.Run();
