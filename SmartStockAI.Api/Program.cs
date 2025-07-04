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
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); 
        });
});
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port)); // OJO: No uses localhost ni loopback
});

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProjectServices(builder.Configuration);

var app = builder.Build();
app.UseRouting();
app.UseCors("_myAllowSpecificOrigins");
app.UseSwagger();
app.UseAuthentication();
app.UseMiddleware<IdleTimeoutMiddleware>(); 
app.UseHangfireDashboard("/hangfire"); 
app.UseAuthorization();
app.UseMiddleware<AuthenticationValidationMiddleware>();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartStockAI API v1");
    c.RoutePrefix = string.Empty; 
});
app.MapControllers();
RecurringJob.AddOrUpdate<IAlertaSistemaService>(
    "verificar-stock-bajo",
    s => s.VerificarProductosConStockBajoAsync(),
    Cron.Hourly);

RecurringJob.AddOrUpdate<IAlertaSistemaService>(
    "verificar-baja-rotacion",
    s => s.VerificarProductosConBajaRotacionAsync(),
    Cron.Daily);
app.Run();