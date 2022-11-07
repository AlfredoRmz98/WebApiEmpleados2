using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiEmpleados2.Entidades;

namespace WebApiEmpleados2
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmpleadoPuesto>()
                .HasKey(al => new { al.EmpleadoId, al.PuestoId });
        }
        public DbSet<Departamentos> Departamentos { get; set; }
        public DbSet<EmpleadoPuesto> EmpleadoPuesto { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Puesto> Puestos { get; set; }
        
    }


}