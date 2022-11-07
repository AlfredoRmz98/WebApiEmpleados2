namespace WebApiEmpleados2.DTOs
{
    public class PuestoDTO
    {
        public int Id { get; set; } 
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<DepartamentoDTO> Departamentos { get; set; }
    }
}
