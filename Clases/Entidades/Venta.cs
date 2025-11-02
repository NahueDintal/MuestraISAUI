using System;
using System.Collections.Generic;
using System.Linq;

namespace MuestraISAUI.Clases.Entidades
{
  public class Venta
  {
    public int IdVenta { get; set; }
    public int CodigoCliente { get; set; }
    public string NombreCliente { get; set; }
    public string SignoZodiacal { get; set; }
    public decimal Total { get; set; }
    public DateTime FechaVenta { get; set; } = DateTime.Now;
    public string LoreAstral { get; set; }
    
    public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();

    public string TotalFormateado => Total.ToString("C2");
    
    public void RecalcularTotal()
    {
      Total = Detalles.Sum(d => d.Subtotal);
    }
    
    public void AgregarDetalle(DetalleVenta detalle)
    {
      Detalles.Add(detalle);
      RecalcularTotal();
    }
  }
}
