using Microsoft.AspNetCore.Identity;

namespace WebApiEmpleados2.Entidades
{
    public class Departamentos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public int PuestoId { get; set; }

        public Puesto Puesto { get; set; }
        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }

    }
}
