namespace WebApiEmpleados2.DTOs
{
    public class EmpleadoDTOConPuestos: GetEmpleadoDTO
    {
        public List<PuestoDTO> Puestos { get; set; }
    }
}
