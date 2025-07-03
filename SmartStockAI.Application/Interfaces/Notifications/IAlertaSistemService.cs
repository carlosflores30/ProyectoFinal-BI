namespace SmartStockAI.Application.Interfaces.Notifications;

public interface IAlertaSistemaService
{
    Task VerificarProductosConStockBajoAsync();
    Task VerificarProductosConBajaRotacionAsync();
    Task GenerarAlertaPorVentaAsync(int idVenta);
    Task GenerarAlertaPorMovimientoAsync(int idMovimiento);
}