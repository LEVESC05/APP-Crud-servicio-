using System.ComponentModel.DataAnnotations;

namespace AppCrud.ViewModels
{
    public class EmpleadoVM
    {
        public int idEmpleado { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Escribe un correo válido.")]
        public string correo { get; set; } = string.Empty;

        public string? Clave { get; set; } = string.Empty;

        public DateOnly FechaContrato { get; set; }

        public bool Activo { get; set; }
    }
}
