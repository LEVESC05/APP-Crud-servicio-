using AppCrud.Services.Services.Authentication;
using AppCrud.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppCrud.Controllers
{
    public class AccesoController : Controller
    {
        private readonly IAutenticacionService _autenticacionService;

        public AccesoController(IAutenticacionService autenticacionService)
        {
            _autenticacionService = autenticacionService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Lista", "Empleado");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {           
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var empleado = await _autenticacionService
                .ValidarUsuario(login.Correo, login.Clave);

            if (empleado == null)
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos");
                return View(login);
            }

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    empleado.idEmpleado.ToString()),
                new Claim(ClaimTypes.Name, empleado.Nombre),
                new Claim(ClaimTypes.Email, empleado.correo)
            };

            var identidad = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            var usuario = new ClaimsPrincipal(identidad);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                usuario);
            return RedirectToAction("Lista", "Empleado");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
