using Microsoft.AspNetCore.Mvc;
using AppCrud.Models;
using AppCrud.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AppCrud.Controllers

{
    public class AccesoController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public AccesoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Empleado empleado)
        {           
            Empleado empleadoEncontrado = await _appDBContext.Empleados.FirstOrDefaultAsync(e => e.correo == empleado.correo);

            if (empleadoEncontrado != null)
            {        
                var passwordHasher = new PasswordHasher<Empleado>();
                var resultado = passwordHasher.VerifyHashedPassword(empleadoEncontrado, empleadoEncontrado.Clave, empleado.Clave);
                if (resultado == PasswordVerificationResult.Success)
                {           
                    if (empleadoEncontrado.Activo)
                    {
                        HttpContext.Session.SetInt32("IdUsuarioLogueado", empleadoEncontrado.idEmpleado);
                        return RedirectToAction("Lista", "Empleado");
                        
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "El usuario no esta activo");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos");
            }
            return View(empleado);
        }
    }
}
