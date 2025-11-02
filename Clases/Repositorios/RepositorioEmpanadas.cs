using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MuestraISAUI.Clases.Entidades;
using MuestraISAUI.Clases.Utilidades;

namespace MuestraISAUI.Clases.Repositorios
{
  public class RepositorioEmpanadas : IRepositorio<Empanada>
  {
    public Empanada ObtenerPorId(int id)
    {
      var sql = "SELECT * FROM Empanada WHERE IdEmpanada = @Id AND Activa = 1";
      var parametros = new[] { new SqlParameter("@Id", id) };
      var tabla = ConexionBD.EjecutarConsulta(sql, parametros);

      return tabla.Rows.Count > 0 ? MapearDesdeFila(tabla.Rows[0]) : null;
    }

    public List<Empanada> ObtenerTodos()
    {
      var empanadas = new List<Empanada>();
      var sql = "SELECT * FROM Empanada WHERE Activa = 1 ORDER BY Nombre";
      var tabla = ConexionBD.EjecutarConsulta(sql);

      foreach (DataRow fila in tabla.Rows)
      {
        empanadas.Add(MapearDesdeFila(fila));
      }
      return empanadas;
    }

    public Empanada ObtenerPorSigno(string signo)
    {
      var sql = "SELECT * FROM Empanada WHERE SignoAsociado = @Signo AND Activa = 1";
      var parametros = new[] { new SqlParameter("@Signo", signo) };
      var tabla = ConexionBD.EjecutarConsulta(sql, parametros);

      return tabla.Rows.Count > 0 ? MapearDesdeFila(tabla.Rows[0]) : null;
    }

    public List<Empanada> BuscarPorNombre(string nombre)
    {
      var empanadas = new List<Empanada>();
      var sql = "SELECT * FROM Empanada WHERE Nombre LIKE @Nombre AND Activa = 1";
      var parametros = new[] { new SqlParameter("@Nombre", $"%{nombre}%") };
      var tabla = ConexionBD.EjecutarConsulta(sql, parametros);

      foreach (DataRow fila in tabla.Rows)
      {
        empanadas.Add(MapearDesdeFila(fila));
      }
      return empanadas;
    }

    public int Insertar(Empanada empanada)
    {
      var sql = @"INSERT INTO Empanada (Nombre, Descripcion, Precio, SignoAsociado, Ingredientes, Activa) 
                  VALUES (@Nombre, @Descripcion, @Precio, @Signo, @Ingredientes, 1);
                  SELECT SCOPE_IDENTITY();";

      var parametros = new[]
      {
        new SqlParameter("@Nombre", empanada.Nombre),
        new SqlParameter("@Descripcion", empanada.Descripcion),
        new SqlParameter("@Precio", empanada.Precio),
        new SqlParameter("@Signo", empanada.SignoAsociado ?? (object)DBNull.Value),
        new SqlParameter("@Ingredientes", empanada.Ingredientes ?? (object)DBNull.Value)
      };

      var resultado = ConexionBD.EjecutarEscalar(sql, parametros);
      return Convert.ToInt32(resultado);
    }

    public void Actualizar(Empanada empanada)
    {
      var sql = @"UPDATE Empanada 
                  SET Nombre = @Nombre, Descripcion = @Descripcion, Precio = @Precio,
                      SignoAsociado = @Signo, Ingredientes = @Ingredientes
                  WHERE IdEmpanada = @Id";

      var parametros = new[]
      {
        new SqlParameter("@Nombre", empanada.Nombre),
        new SqlParameter("@Descripcion", empanada.Descripcion),
        new SqlParameter("@Precio", empanada.Precio),
        new SqlParameter("@Signo", empanada.SignoAsociado ?? (object)DBNull.Value),
        new SqlParameter("@Ingredientes", empanada.Ingredientes ?? (object)DBNull.Value),
        new SqlParameter("@Id", empanada.IdEmpanada)
      };

      ConexionBD.EjecutarComando(sql, parametros);
    }

    public void Eliminar(int id)
    {
      var sql = "UPDATE Empanada SET Activa = 0 WHERE IdEmpanada = @Id";
      var parametros = new[] { new SqlParameter("@Id", id) };
      ConexionBD.EjecutarComando(sql, parametros);
    }

    public bool Existe(int id)
    {
      var sql = "SELECT COUNT(*) FROM Empanada WHERE IdEmpanada = @Id AND Activa = 1";
      var parametros = new[] { new SqlParameter("@Id", id) };
      var resultado = ConexionBD.EjecutarEscalar(sql, parametros);
      return Convert.ToInt32(resultado) > 0;
    }

    private Empanada MapearDesdeFila(DataRow fila)
    {
      return new Empanada
      {
        IdEmpanada = Convert.ToInt32(fila["IdEmpanada"]),
        Nombre = fila["Nombre"].ToString(),
        Descripcion = fila["Descripcion"].ToString(),
        Precio = Convert.ToDecimal(fila["Precio"]),
        SignoAsociado = fila["SignoAsociado"]?.ToString(),
        Ingredientes = fila["Ingredientes"]?.ToString(),
        EstaActiva = Convert.ToBoolean(fila["Activa"])
      };
    }
  }
}
