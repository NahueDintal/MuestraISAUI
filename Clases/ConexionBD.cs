using System;
using System.Data;
using System.Data.SqlClient;

namespace MuestraISAUI.Clases
{
  public static class ConexionBD
  {
    private static string connectionString = "";

    public static DataTable EjecutarConsulta(string sql)
    {
      using (SqlConnection conexion = new SqlConnection(connectionString))
      {
        SqlCommand comando = new SqlCommand(Sql, conexion);
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
  }
}
