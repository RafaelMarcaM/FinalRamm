using System;
using System.Collections.Generic;

namespace WebFinalRamm.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string UnidadMedida { get; set; } = null!;

    public decimal Existencias { get; set; }

    public string Marca { get; set; } = null!;

    public decimal PrecioVenta { get; set; }

    public string? UsuarioRegistro { get; set; }

    public bool? RegistroActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<CompraDetalle> CompraDetalles { get; set; } = new List<CompraDetalle>();

    public virtual ICollection<VentaDetalle> VentaDetalles { get; set; } = new List<VentaDetalle>();
}
