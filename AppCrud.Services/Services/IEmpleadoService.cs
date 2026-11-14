using AppCrud.Models;

namespace AppCrud.Services
{
    public interface IEmpleadoService
    {
        Task<List<Empleado>> ObtenerTodos();
        Task<Empleado?> ObtenerPorId(int id);
        Task Crear(Empleado empleado);
        Task<bool> Editar(int id, Empleado empleado);
        Task<bool> Eliminar(int id);
    }
}
