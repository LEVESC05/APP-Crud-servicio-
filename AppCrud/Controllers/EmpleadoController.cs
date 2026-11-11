using AppCrud.Data;
using AppCrud.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AppCrud.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public EmpleadoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Empleado> lista = await _appDBContext.Empleados.ToListAsync();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo(Empleado empleado)
        {
            var passwordHasher = new PasswordHasher<Empleado>();
            empleado.Clave = passwordHasher.HashPassword(empleado, empleado.Clave);
            await _appDBContext.Empleados.AddAsync(empleado);
            await _appDBContext.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Empleado empleado = await _appDBContext.Empleados.FirstAsync(e => e.idEmpleado == id);
            return View(empleado);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Empleado empleado)
        {
            if (!string.IsNullOrEmpty(empleado.Clave))
            {
                var passwordHasher = new PasswordHasher<Empleado>();
                empleado.Clave = passwordHasher.HashPassword(empleado, empleado.Clave);
            }
            else
            {
                var empleadoExistente = await _appDBContext.Empleados.AsNoTracking().FirstAsync(e => e.idEmpleado == empleado.idEmpleado);
                empleado.Clave = empleadoExistente.Clave;
            }
            _appDBContext.Empleados.Update(empleado);
            await _appDBContext.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
           
            int? idSesion = HttpContext.Session.GetInt32("IdUsuarioLogueado");

            if (idSesion != null && idSesion == id)
            {
                TempData["Error"] = "No te puedes eliminar a ti mismo";
                return RedirectToAction(nameof(Lista));
            }

            Empleado empleado = await _appDBContext.Empleados.FirstAsync(e => e.idEmpleado == id);
            _appDBContext.Empleados.Remove(empleado);
            await _appDBContext.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<IActionResult> CerrarSesion()
        {
            return RedirectToAction("Login", "Acceso");
        }
    }
}