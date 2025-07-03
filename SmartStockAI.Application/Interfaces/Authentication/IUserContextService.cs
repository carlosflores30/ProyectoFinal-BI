namespace SmartStockAI.Application.Interfaces.Authentication;

public interface IUserContextService
{
    int GetNegocioId();
    int GetUsuarioId();
    string? GetEmail();
    string? GetRol();
}