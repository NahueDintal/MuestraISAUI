namespace MuestraISAUI.Clases.Entidades
{
  public class Empanada
  {
    public int IdEmpanada { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal Precio { get; set; }
    public string SignoAsociado { get; set; }
    public string Ingredientes { get; set; }
    public bool EstaActiva { get; set; } = true;

    public string PrecioFormateado => Precio.ToString("C2");
    public string InformacionCompleta => $"{Nombre} - {PrecioFormateado}";
    
    public Empanada() { }
    
    public Empanada(string nombre, string descripcion, decimal precio, string signoAsociado)
    {
      Nombre = nombre;
      Descripcion = descripcion;
      Precio = precio;
      SignoAsociado = signoAsociado;
    }
  }
}
