using System;

namespace MuestraISAUI.Clases.Entidades
{
  public class Cliente
  {
    public int CodigoCliente { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string NumeroDocumento { get; set; }
    public string SignoZodiacal { get; set; }
    public string EmpanadaDestino { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public string NombreCompleto => $"{Nombre} {Apellido}";
    
    public Cliente() { }
    
    public Cliente(string nombre, string apellido, string signoZodiacal)
    {
      Nombre = nombre;
      Apellido = apellido;
      SignoZodiacal = signoZodiacal;
    }
  }
}
