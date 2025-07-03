using System;
using System.Collections.Generic;

namespace SmartStockAI.Infrastructure.Persistence.Models;

public partial class PasswordResetTokens
{
    public Guid Id { get; set; }

    public int IdUsuario { get; set; }

    public string Token { get; set; } = null!;

    public DateTime Expiration { get; set; }

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
}
