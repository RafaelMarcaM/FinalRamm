using System;
using System.Collections.Generic;

namespace WebFinalRamm.Models;

public partial class Proveedor
{
    public int Id { get; set; }

    public long Nit { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string? Direccion { get; set; }

    public string Telefono { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string Banco { get; set; } = null!;

    public decimal CuentaBancaria { get; set; }

    public string? UsuarioRegistro { get; set; }

    public bool? RegistroActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
