namespace AppCrud.Services.Services.Authentication
{
    public interface IAutenticacionService
    {
        Task<AppCrud.Models.Empleado?> ValidarUsuario(string correo, string clave);
    }
}