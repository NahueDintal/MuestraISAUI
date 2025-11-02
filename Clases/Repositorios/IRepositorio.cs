using System.Collections.Generic;

namespace MuestraISAUI.Clases.Repositorios
{
  public interface IRepositorio<T>
  {
    T ObtenerPorId(int id);
    List<T> ObtenerTodos();
    int Insertar(T entidad);
    void Actualizar(T entidad);
    void Eliminar(int id);
    bool Existe(int id);
  }
}
