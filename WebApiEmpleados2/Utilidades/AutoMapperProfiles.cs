using AutoMapper;
using WebApiEmpleados2.DTOs;
using WebApiEmpleados2.Entidades;

namespace WebApiAlumnosSeg.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //Establecemos el mappeo de EmpleadoDTO hacia la entidad Empleado
            CreateMap<EmpleadoDTO, Empleado>();
            //Establecemos el mappeo de la entidad Empleado hacia la el DTO GetEmpleadoDTO
            CreateMap<Empleado, GetEmpleadoDTO>();
            //Establecemos el mappeo desde la entidad Empleado hacia el DTO EmpleadoDTOConPuestos
            CreateMap<Empleado, EmpleadoDTOConPuestos>()
                .ForMember(empleadoDTO => empleadoDTO.Puestos, opciones => opciones.MapFrom(MapEmpleadoDTOPuestos));
            //Establecemos el mappeo de PuestoCreacionDTO hacia nuestra Entidad Puesto
            CreateMap<PuestoCreacionDTO, Puesto>()
                .ForMember(puesto => puesto.EmpleadoPuesto, opciones => opciones.MapFrom(MapEmpleadoPuesto));
            //Establecemos el mappeo de la Entidad Puesto hacia el DTO PuestoDTO
            CreateMap<Puesto, PuestoDTO>();
            //Establecemos el mappeo de la Entidad Puesto hacie el DTO PuestoDTOconEmpleados
            CreateMap<Puesto, PuestoDTOconEmpleados>()
                .ForMember(puestoDTO => puestoDTO.Empleados, opciones => opciones.MapFrom(MapPuestoDTOEmpleados));
            //Establecemos el mappeo des PuestoPatchDTO hacia la entidad Puesto
            CreateMap<PuestoPatchDTO, Puesto>().ReverseMap();
            //Establecemos el mappeo desde DepartamentoCreacionDTO hacia la Entidad Departamentos
            CreateMap<DepartamentoCreacionDTO, Departamentos>();
            //Establecemos el mappeo desde la Entidad Departamentos hacia el DTO DepartamentoDTO
            CreateMap<Departamentos, DepartamentoDTO>();

        }
        //Se crea una lista PuestoDTO con los parametros empleado y getEmpleadoDTO
        private List<PuestoDTO> MapEmpleadoDTOPuestos(Empleado empleado, GetEmpleadoDTO getEmpleadoDTO)
        {
            var result = new List<PuestoDTO>();
            //Verificamos que no sea null
            if (empleado.EmpleadoPuesto == null) { return result; }
            //iteraciones
            foreach (var empleadoPuesto in empleado.EmpleadoPuesto)
            {
                result.Add(new PuestoDTO()
                {
                    Id = empleadoPuesto.PuestoId,
                    Nombre = empleadoPuesto.Puesto.Nombre
                });
            }

            return result;
        }

        //Se crean una lista de GetEmpleadoDTO
        private List<GetEmpleadoDTO> MapPuestoDTOEmpleados(Puesto puesto, PuestoDTO puestoDTO)
        {
            
            var result = new List<GetEmpleadoDTO>();
            //Buscamos si es null
            if (puesto.EmpleadoPuesto == null)
            {
                return result;
            }
            //iteraciones
            foreach (var empleadopuesto in puesto.EmpleadoPuesto)
            {
                result.Add(new GetEmpleadoDTO()
                {
                    Id = empleadopuesto.EmpleadoId,
                    Nombre = empleadopuesto.Empleado.Nombre
                });
            }

            return result;
        }

        //Se crea una lista de EmpleadoPuesto
        private List<EmpleadoPuesto> MapEmpleadoPuesto(PuestoCreacionDTO puestoCreacionDTO, Puesto puesto)
        {
            
            var resultado = new List<EmpleadoPuesto>();
            //se verifica si es null o no
            if (puestoCreacionDTO.EmpleadosIds == null)
            {
                return resultado;
            }

            //iteraciones
            foreach (var empleadoId in puestoCreacionDTO.EmpleadosIds)
            {
                resultado.Add(new EmpleadoPuesto() { EmpleadoId = empleadoId });
            }
            return resultado;
        }
    }
}