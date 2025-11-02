using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MuestraISAUI.Clases.Utilidades
{
  public static class ConexionBD
  {
    private static string connectionString = "";

    public static DataTable EjecutarConsulta(string sql)
    {
      using (SqlConnection conexion = new SqlConnection(connectionString))
      {
        SqlCommand comando = new SqlCommand(sql, conexion);
        SqlDataAdapter adaptador = new SqlDataAdapter(comando);
        DataTable tabla = new DataTable();
        adaptador.Fill(tabla);
        return tabla;
      }
    }

    public static void EjecutarComando(string sql)
    {
      using (SqlConnection conexion = new SqlConnection(connectionString))
      {
        conexion.Open();
        SqlCommand comando = new SqlCommand(sql, conexion);
        comando.ExecuteNonQuery();
      }
    }
    public static object EjecutarEscalar(string sql, params SqlParameter[] parametros)
    {
      using (var conexion = new SqlConnection(connectionString))
      using (var comando = new SqlCommand(sql, conexion))
      {
        comando.Parameters.AddRange(parametros);
        conexion.Open();
        return comando.ExecuteScalar();
      }
    }
  }
}
