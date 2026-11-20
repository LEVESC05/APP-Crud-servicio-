namespace AppCrud.Services.Services.Empleado
{
    public interface IEmpleadoService
    {
        Task<List<AppCrud.Models.Empleado>> ObtenerTodos();
        Task<AppCrud.Models.Empleado?> ObtenerPorId(int id);
        Task Crear(AppCrud.Models.Empleado empleado);
        Task<bool> Editar(int id, AppCrud.Models.Empleado empleado);
        Task<bool> Eliminar(int id);
    }
}