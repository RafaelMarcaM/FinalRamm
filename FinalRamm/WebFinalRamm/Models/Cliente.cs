using System;
using System.Collections.Generic;

namespace WebFinalRamm.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public long Nit { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string? UsuarioRegistro { get; set; }

    public bool? RegistroActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
