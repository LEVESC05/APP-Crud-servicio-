using AppCrud.Models;

namespace AppCrud.Services
{
    public interface IAutenticacionService
    {
        Task<Empleado?> ValidarUsuario(string correo, string clave);
    }
}
