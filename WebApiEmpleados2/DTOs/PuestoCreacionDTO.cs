using System.ComponentModel.DataAnnotations;
using WebApiAlumnos2.Validaciones;

namespace WebApiEmpleados2.DTOs
{
    public class PuestoCreacionDTO
    {
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} solo puede tener hasta 250 caracteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<int> EmpleadosIds { get; set; }
    }
}
