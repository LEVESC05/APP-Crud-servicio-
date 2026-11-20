using AppCrud.Data;
using AppCrud.Models;
using AppCrud.Services.Services.Password;
using Microsoft.EntityFrameworkCore;

namespace AppCrud.Services.Services.Empleado
{
    using Empleado = AppCrud.Models.Empleado;

    public class EmpleadoService : IEmpleadoService
    {
        private readonly AppDBContext _appDBContext;
        private readonly IPasswordService _passwordService;

        public EmpleadoService(
            AppDBContext appDBContext,
            IPasswordService passwordService)
        {
            _appDBContext = appDBContext;
            _passwordService = passwordService;
        }

        public async Task<List<Empleado>> ObtenerTodos()
        {
            return await _appDBContext.Empleados.ToListAsync();
        }

        public async Task<Empleado?> ObtenerPorId(int id)
        {
            return await _appDBContext.Empleados
                .FirstOrDefaultAsync(empleado => empleado.idEmpleado == id);
        }

        public async Task Crear(Empleado empleado)
        {
            empleado.Clave = _passwordService.HashPassword(empleado.Clave);

            await _appDBContext.Empleados.AddAsync(empleado);
            await _appDBContext.SaveChangesAsync();
        }

        public async Task<bool> Editar(int id, Empleado empleado)
        {
            Empleado? empleadoGuardado = await ObtenerPorId(id);

            if (empleadoGuardado == null)
            {
                return false;
            }

            empleadoGuardado.Nombre = empleado.Nombre;
            empleadoGuardado.correo = empleado.correo;
            empleadoGuardado.FechaContrato = empleado.FechaContrato;
            empleadoGuardado.Activo = empleado.Activo;

            if (!string.IsNullOrWhiteSpace(empleado.Clave))
            {
                empleadoGuardado.Clave =
                    _passwordService.HashPassword(empleado.Clave);
            }

            await _appDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            Empleado? empleado = await ObtenerPorId(id);

            if (empleado == null)
            {
                return false;
            }

            _appDBContext.Empleados.Remove(empleado);
            await _appDBContext.SaveChangesAsync();
            return true;
        }
    }
}