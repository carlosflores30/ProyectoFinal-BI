﻿using System;
using System.Collections.Generic;

namespace SmartStockAI.Infrastructure.Persistence.Models;

public partial class Roles
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
}
