using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MuestraISAUI.Clases.Entidades;
using MuestraISAUI.Clases.Utilidades;

namespace MuestraISAUI.Clases.Repositorios
{
  public class RepositorioClientes : IRepositorio<Cliente>
  {
    public Cliente ObtenerPorId(int id)
    {
      var sql = "SELECT * FROM Cliente WHERE CodCliente = @Id";
      var parametros = new[] { new SqlParameter("@Id", id) };
      var tabla = ConexionBD.EjecutarConsulta(sql, parametros);

      return tabla.Rows.Count > 0 ? MapearDesdeFila(tabla.Rows[0]) : null;
    }

    public List<Cliente> ObtenerTodos()
    {
      var clientes = new List<Cliente>();
      var sql = "SELECT * FROM Cliente ORDER BY Apellido, Nombre";
      var tabla = ConexionBD.EjecutarConsulta(sql);

      foreach (DataRow fila in tabla.Rows)
      {
        clientes.Add(MapearDesdeFila(fila));
      }
      return clientes;
    }

    public List<Cliente> BuscarPorApellido(string apellido)
    {
      var clientes = new List<Cliente>();
      var sql = "SELECT * FROM Cliente WHERE Apellido LIKE @Apellido ORDER BY Apellido, Nombre";
      var parametros = new[] { new SqlParameter("@Apellido", $"%{apellido}%") };
      var tabla = ConexionBD.EjecutarConsulta(sql, parametros);

      foreach (DataRow fila in tabla.Rows)
      {
        clientes.Add(MapearDesdeFila(fila));
      }
      return clientes;
    }

    public List<Cliente> BuscarPorSigno(string signo)
    {
      var clientes = new List<Cliente>();
      var sql = "SELECT * FROM Cliente WHERE SignoZodiacal = @Signo ORDER BY Apellido, Nombre";
      var parametros = new[] { new SqlParameter("@Signo", signo) };
      var tabla = ConexionBD.EjecutarConsulta(sql, parametros);

      foreach (DataRow fila in tabla.Rows)
      {
        clientes.Add(MapearDesdeFila(fila));
      }
      return clientes;
    }

    public int Insertar(Cliente cliente)
    {
      var sql = @"INSERT INTO Cliente (Nombre, Apellido, NroDoc, SignoZodiacal, EmpanadaDestino, FechaRegistro) 
                VALUES (@Nombre, @Apellido, @NroDoc, @Signo, @Empanada, @Fecha);
                SELECT SCOPE_IDENTITY();";

      var parametros = new[]
      {
        new SqlParameter("@Nombre", cliente.Nombre),
        new SqlParameter("@Apellido", cliente.Apellido),
        new SqlParameter("@NroDoc", cliente.NumeroDocumento ?? (object)DBNull.Value),
        new SqlParameter("@Signo", cliente.SignoZodiacal ?? (object)DBNull.Value),
        new SqlParameter("@Empanada", cliente.EmpanadaDestino ?? (object)DBNull.Value),
        new SqlParameter("@Fecha", cliente.FechaRegistro)
      };

      var resultado = ConexionBD.EjecutarEscalar(sql, parametros);
      return Convert.ToInt32(resultado);
    }

    public void Actualizar(Cliente cliente)
    {
      var sql = @"UPDATE Cliente 
                SET Nombre = @Nombre, Apellido = @Apellido, NroDoc = @NroDoc,
                    SignoZodiacal = @Signo, EmpanadaDestino = @Empanada
                WHERE CodCliente = @Id";

      var parametros = new[]
      {
        new SqlParameter("@Nombre", cliente.Nombre),
        new SqlParameter("@Apellido", cliente.Apellido),
        new SqlParameter("@NroDoc", cliente.NumeroDocumento ?? (object)DBNull.Value),
        new SqlParameter("@Signo", cliente.SignoZodiacal ?? (object)DBNull.Value),
        new SqlParameter("@Empanada", cliente.EmpanadaDestino ?? (object)DBNull.Value),
        new SqlParameter("@Id", cliente.CodigoCliente)
      };

      ConexionBD.EjecutarComando(sql, parametros);
    }

    public void Eliminar(int id)
    {
      var sql = "DELETE FROM Cliente WHERE CodCliente = @Id";
      var parametros = new[] { new SqlParameter("@Id", id) };
      ConexionBD.EjecutarComando(sql, parametros);
    }

    public bool Existe(int id)
    {
      var sql = "SELECT COUNT(*) FROM Cliente WHERE CodCliente = @Id";
      var parametros = new[] { new SqlParameter("@Id", id) };
      var resultado = ConexionBD.EjecutarEscalar(sql, parametros);
      return Convert.ToInt32(resultado) > 0;
    }

    private Cliente MapearDesdeFila(DataRow fila)
    {
      return new Cliente
      {
        CodigoCliente = Convert.ToInt32(fila["CodCliente"]),
        Nombre = fila["Nombre"].ToString(),
        Apellido = fila["Apellido"].ToString(),
        NumeroDocumento = fila["NroDoc"]?.ToString(),
        SignoZodiacal = fila["SignoZodiacal"]?.ToString(),
        EmpanadaDestino = fila["EmpanadaDestino"]?.ToString(),
        FechaRegistro = Convert.ToDateTime(fila["FechaRegistro"])
      };
    }
  }
}
