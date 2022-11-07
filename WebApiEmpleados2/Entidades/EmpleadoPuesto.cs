namespace WebApiEmpleados2.Entidades
{
    public class EmpleadoPuesto
    {
        public int EmpleadoId { get; set; }
        public int PuestoId { get; set; }
        public int Orden { get; set; }
        public Empleado Empleado { get; set; }
        public Puesto Puesto { get; set; }
    }
}
