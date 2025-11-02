namespace MuestraISAUI.Clases.Entidades
{
  public class DetalleVenta
  {
    public Empanada Producto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    
    public decimal Subtotal => Cantidad * PrecioUnitario;

    public string NombreProducto => Producto?.Nombre ?? "No disponible";
    public string PrecioFormateado => PrecioUnitario.ToString("C2");
    public string SubtotalFormateado => Subtotal.ToString("C2");
    public string SignoAsociado => Producto?.SignoAsociado ?? "Sin signo";
    
    public DetalleVenta() { }
    
    public DetalleVenta(Empanada producto, int cantidad)
    {
      Producto = producto;
      Cantidad = cantidad;
      PrecioUnitario = producto.Precio;
    }
  }
}
