﻿using System;
using System.Collections.Generic;

namespace WebFinalRamm.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public int IdEmpleado { get; set; }

    public string Usuario1 { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public string? UsuarioRegistro { get; set; }

    public bool? RegistroActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
