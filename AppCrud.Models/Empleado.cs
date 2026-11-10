using System.ComponentModel.DataAnnotations;

namespace AppCrud.Models
{
    public class Empleado
    {
        public int idEmpleado { get; set; }
        public string Nombre { get; set; }
        public string correo { get; set; }
        public DateOnly FechaContrato { get; set; }
        public string Clave{ get; set; }
        public bool Activo { get; set; }
    }
}
