using System;
using System.Collections.Generic;
using MuestraISAUI.Clases.Entidades;
using MuestraISAUI.Clases.Repositorios;

namespace MuestraISAUI.Clases.Servicios
{
  public class ServicioVentas
  {
    private readonly RepositorioVentas _repositorioVentas;
    private readonly RepositorioClientes _repositorioClientes;
    private readonly ServicioZodiacal _servicioZodiacal;

    public ServicioVentas()
    {
      _repositorioVentas = new RepositorioVentas();
      _repositorioClientes = new RepositorioClientes();
      _servicioZodiacal = new ServicioZodiacal();
    }

    public int ProcesarVentaEpica(string nombreCliente, string apellidoCliente, string documento, string signoZodiacal)
    {
      try
      {
        var (empanadaDestino, lore) = _servicioZodiacal.DescubrirDestinoEmpanaderil(signoZodiacal);
        
        var cliente = new Cliente(nombreCliente, apellidoCliente, signoZodiacal)
        {
            NumeroDocumento = documento,
            EmpanadaDestino = empanadaDestino.Nombre
        };
        
        var idCliente = _repositorioClientes.Insertar(cliente);
        
        var venta = new Venta
        {
            CodigoCliente = idCliente,
            NombreCliente = $"{nombreCliente} {apellidoCliente}",
            SignoZodiacal = signoZodiacal,
            LoreAstral = lore
        };
        
        var detalle = new DetalleVenta(empanadaDestino, 1);
        venta.AgregarDetalle(detalle);
        
        var idVenta = _repositorioVentas.Insertar(venta);
        venta.IdVenta = idVenta;
        
        var ticket = _servicioZodiacal.GenerarTicketEpico(venta);
        MostrarTicket(ticket);
        
        return idVenta;
      }
      catch (Exception ex)
      {
        throw new Exception($"Error cósmico al procesar la venta: {ex.Message}", ex);
      }
    }

    public List<Venta> ObtenerHistorialVentas()
    {
        return _repositorioVentas.ObtenerTodos();
    }

    public List<Venta> ObtenerVentasPorSigno(string signo)
    {
        var todasVentas = _repositorioVentas.ObtenerTodos();
        return todasVentas.FindAll(v => v.SignoZodiacal.Equals(signo, StringComparison.OrdinalIgnoreCase));
    }

    public Dictionary<string, int> ObtenerEstadisticasPorSigno()
    {
      var estadisticas = new Dictionary<string, int>();
      var ventas = _repositorioVentas.ObtenerTodos();
      
      foreach (var venta in ventas)
      {
        if (estadisticas.ContainsKey(venta.SignoZodiacal))
        {
          estadisticas[venta.SignoZodiacal]++;
        }
        else
        {
          estadisticas[venta.SignoZodiacal] = 1;
        }
      }
      
      return estadisticas;
    }

    private void MostrarTicket(string ticket)
    {
      System.Diagnostics.Debug.WriteLine(ticket);
    }
  }
}
