using AppCrud.Data;
using AppCrud.Services.Services.Password;
using Microsoft.EntityFrameworkCore;

namespace AppCrud.Services.Services.Authentication
{
    using Empleado = AppCrud.Models.Empleado;

    public class AutenticacionService : IAutenticacionService
    {
        private readonly AppDBContext _appDBContext;
        private readonly IPasswordService _passwordService;

        public AutenticacionService(
            AppDBContext appDBContext,
            IPasswordService passwordService)
        {
            _appDBContext = appDBContext;
            _passwordService = passwordService;
        }

        public async Task<Empleado?> ValidarUsuario(string correo, string clave)
        {
            Empleado? empleado = await _appDBContext.Empleados
                .FirstOrDefaultAsync(e => e.correo == correo && e.Activo);

            if (empleado == null ||
                !_passwordService.VerifyPassword(clave, empleado.Clave))
            {
                return null;
            }

            if (empleado.Clave == clave)
            {
                empleado.Clave = _passwordService.HashPassword(clave);
                await _appDBContext.SaveChangesAsync();
            }

            return empleado;
        }
    }
}