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
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProjectServices(builder.Configuration);

var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();
app.UseSwagger();
app.UseAuthentication();
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