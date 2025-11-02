using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MuestraISAUI.Clases.Entidades;
using MuestraISAUI.Clases.Utilidades;

namespace MuestraISAUI.Clases.Repositorios
{
  public class RepositorioVentas : IRepositorio<Venta>
  {
    private readonly RepositorioEmpanadas _repositorioEmpanadas;

    public RepositorioVentas()
    {
      _repositorioEmpanadas = new RepositorioEmpanadas();
    }

    public Venta ObtenerPorId(int id)
    {
      var sql = "SELECT * FROM Venta WHERE IdVenta = @Id";
      var parametros = new[] { new SqlParameter("@Id", id) };
      var tabla = ConexionBD.EjecutarConsulta(sql, parametros);

      if (tabla.Rows.Count == 0) return null;

      var venta = MapearVentaDesdeFila(tabla.Rows[0]);
      venta.Detalles = ObtenerDetallesVenta(id);
      
      return venta;
    }

    public List<Venta> ObtenerTodos()
    {
      var ventas = new List<Venta>();
      var sql = "SELECT * FROM Venta ORDER BY FechaVenta DESC";
      var tabla = ConexionBD.EjecutarConsulta(sql);

      foreach (DataRow fila in tabla.Rows)
      {
        var venta = MapearVentaDesdeFila(fila);
        venta.Detalles = ObtenerDetallesVenta(venta.IdVenta);
        ventas.Add(venta);
      }
      return ventas;
    }

    public int Insertar(Venta venta)
    {
      var sqlVenta = @"INSERT INTO Venta (CodCliente, NombreCliente, SignoZodiacal, Total, FechaVenta, LoreAstral) 
                      VALUES (@CodCliente, @NombreCliente, @Signo, @Total, @Fecha, @Lore);
                      SELECT SCOPE_IDENTITY();";

      var parametrosVenta = new[]
      {
        new SqlParameter("@CodCliente", venta.CodigoCliente),
        new SqlParameter("@NombreCliente", venta.NombreCliente),
        new SqlParameter("@Signo", venta.SignoZodiacal),
        new SqlParameter("@Total", venta.Total),
        new SqlParameter("@Fecha", venta.FechaVenta),
        new SqlParameter("@Lore", venta.LoreAstral ?? (object)DBNull.Value)
      };

      var idVenta = Convert.ToInt32(ConexionBD.EjecutarEscalar(sqlVenta, parametrosVenta));

      foreach (var detalle in venta.Detalles)
      {
        var sqlDetalle = @"INSERT INTO DetalleVenta (IdVenta, IdEmpanada, Cantidad, PrecioUnitario, Subtotal)
                          VALUES (@IdVenta, @IdEmpanada, @Cantidad, @Precio, @Subtotal)";

        var parametrosDetalle = new[]
        {
          new SqlParameter("@IdVenta", idVenta),
          new SqlParameter("@IdEmpanada", detalle.Producto.IdEmpanada),
          new SqlParameter("@Cantidad", detalle.Cantidad),
          new SqlParameter("@Precio", detalle.PrecioUnitario),
          new SqlParameter("@Subtotal", detalle.Subtotal)
        };

        ConexionBD.EjecutarComando(sqlDetalle, parametrosDetalle);
      }

      return idVenta;
    }

    public void Actualizar(Venta venta)
    {
      var sqlVenta = @"UPDATE Venta 
                      SET CodCliente = @CodCliente, NombreCliente = @NombreCliente, 
                          SignoZodiacal = @Signo, Total = @Total, LoreAstral = @Lore
                      WHERE IdVenta = @Id";

      var parametrosVenta = new[]
      {
        new SqlParameter("@CodCliente", venta.CodigoCliente),
        new SqlParameter("@NombreCliente", venta.NombreCliente),
        new SqlParameter("@Signo", venta.SignoZodiacal),
        new SqlParameter("@Total", venta.Total),
        new SqlParameter("@Lore", venta.LoreAstral ?? (object)DBNull.Value),
        new SqlParameter("@Id", venta.IdVenta)
      };

      ConexionBD.EjecutarComando(sqlVenta, parametrosVenta);

      var sqlEliminarDetalles = "DELETE FROM DetalleVenta WHERE IdVenta = @IdVenta";
      ConexionBD.EjecutarComando(sqlEliminarDetalles, new SqlParameter("@IdVenta", venta.IdVenta));

      foreach (var detalle in venta.Detalles)
      {
        var sqlDetalle = @"INSERT INTO DetalleVenta (IdVenta, IdEmpanada, Cantidad, PrecioUnitario, Subtotal)
                          VALUES (@IdVenta, @IdEmpanada, @Cantidad, @Precio, @Subtotal)";

        var parametrosDetalle = new[]
        {
          new SqlParameter("@IdVenta", venta.IdVenta),
          new SqlParameter("@IdEmpanada", detalle.Producto.IdEmpanada),
          new SqlParameter("@Cantidad", detalle.Cantidad),
          new SqlParameter("@Precio", detalle.PrecioUnitario),
          new SqlParameter("@Subtotal", detalle.Subtotal)
        };

        ConexionBD.EjecutarComando(sqlDetalle, parametrosDetalle);
      }
    }

    public void Eliminar(int id)
    {
      var sqlDetalles = "DELETE FROM DetalleVenta WHERE IdVenta = @Id";
      ConexionBD.EjecutarComando(sqlDetalles, new SqlParameter("@Id", id));

      var sqlVenta = "DELETE FROM Venta WHERE IdVenta = @Id";
      ConexionBD.EjecutarComando(sqlVenta, new SqlParameter("@Id", id));
    }

    public bool Existe(int id)
    {
      var sql = "SELECT COUNT(*) FROM Venta WHERE IdVenta = @Id";
      var parametros = new[] { new SqlParameter("@Id", id) };
      var resultado = ConexionBD.EjecutarEscalar(sql, parametros);
      return Convert.ToInt32(resultado) > 0;
    }

    private List<DetalleVenta> ObtenerDetallesVenta(int idVenta)
    {
      var detalles = new List<DetalleVenta>();
      var sql = "SELECT * FROM DetalleVenta WHERE IdVenta = @IdVenta";
      var parametros = new[] { new SqlParameter("@IdVenta", idVenta) };
      var tabla = ConexionBD.EjecutarConsulta(sql, parametros);

      foreach (DataRow fila in tabla.Rows)
      {
        var idEmpanada = Convert.ToInt32(fila["IdEmpanada"]);
        var empanada = _repositorioEmpanadas.ObtenerPorId(idEmpanada);

        if (empanada != null)
        {
          var detalle = new DetalleVenta
          {
              Producto = empanada,
              Cantidad = Convert.ToInt32(fila["Cantidad"]),
              PrecioUnitario = Convert.ToDecimal(fila["PrecioUnitario"])
          };
          detalles.Add(detalle);
        }
      }

      return detalles;
    }

    private Venta MapearVentaDesdeFila(DataRow fila)
    {
      return new Venta
      {
        IdVenta = Convert.ToInt32(fila["IdVenta"]),
        CodigoCliente = Convert.ToInt32(fila["CodCliente"]),
        NombreCliente = fila["NombreCliente"].ToString(),
        SignoZodiacal = fila["SignoZodiacal"].ToString(),
        Total = Convert.ToDecimal(fila["Total"]),
        FechaVenta = Convert.ToDateTime(fila["FechaVenta"]),
        LoreAstral = fila["LoreAstral"]?.ToString()
      };
    }
  }
}
