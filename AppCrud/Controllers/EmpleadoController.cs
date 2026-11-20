using AppCrud.Models;
using AppCrud.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AppCrud.Services.Services.Empleado;

namespace AppCrud.Controllers
{
    [Authorize]
    public class EmpleadoController : Controller
    {
        private readonly IEmpleadoService _empleadoService;

        public EmpleadoController(IEmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Empleado> lista = await _empleadoService.ObtenerTodos();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Nuevo()
        {
            return View(new EmpleadoVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nuevo(EmpleadoVM empleadoVM)
        {
            if (string.IsNullOrWhiteSpace(empleadoVM.Clave))
            {
                ModelState.AddModelError(
                    nameof(empleadoVM.Clave),
                    "La contraseña es obligatoria.");
            }

            if (!ModelState.IsValid)
            {
                return View(empleadoVM);
            }

            var empleado = new Empleado
            {
                Nombre = empleadoVM.Nombre,
                correo = empleadoVM.correo,
                Clave = empleadoVM.Clave!,
                FechaContrato = empleadoVM.FechaContrato,
                Activo = empleadoVM.Activo
            };

            await _empleadoService.Crear(empleado);
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Empleado? empleado = await _empleadoService.ObtenerPorId(id);

            if (empleado == null)
            {
                return NotFound();
            }

            var empleadoVM = new EmpleadoVM
            {
                idEmpleado = empleado.idEmpleado,
                Nombre = empleado.Nombre,
                correo = empleado.correo,
                FechaContrato = empleado.FechaContrato,
                Activo = empleado.Activo
            };

            return View(empleadoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(EmpleadoVM empleadoVM)
        {
            if (!ModelState.IsValid)
            {
                return View(empleadoVM);
            }

            var empleado = new Empleado
            {
                idEmpleado = empleadoVM.idEmpleado,
                Nombre = empleadoVM.Nombre,
                correo = empleadoVM.correo,
                Clave = empleadoVM.Clave ?? string.Empty,
                FechaContrato = empleadoVM.FechaContrato,
                Activo = empleadoVM.Activo
            };

            bool actualizado = await _empleadoService.Editar(
                empleadoVM.idEmpleado,
                empleado);

            if (!actualizado)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            string? idUsuario = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (idUsuario == id.ToString())
            {
                TempData["Error"] = "No te puedes eliminar a ti mismo";
                return RedirectToAction(nameof(Lista));
            }

            bool eliminado = await _empleadoService.Eliminar(id);
            return eliminado ? RedirectToAction(nameof(Lista)) : NotFound();
        }
    }
}