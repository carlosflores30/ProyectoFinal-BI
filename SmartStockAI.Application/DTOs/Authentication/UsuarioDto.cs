using SmartStockAI.Application.DTOs.Negocios;

namespace SmartStockAI.Application.DTOs.Authentication;

public class UsuarioDto
{
    public int Id { get; set; } // ← generado por la BD
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public int RoleId { get; set; }
    public int? NegocioId { get; set; }

    
    public NegocioMiniDto? Negocio { get; set; }
}