namespace Inmobiliaria.Models
{
    public interface IRepositorio<T>
    {
        int Alta(T p);
        int Eliminar(int id);
        int Modificar(T p);

        List<T> ObtenerTodos();
        T ObtenerPorId(int id);
    }
}