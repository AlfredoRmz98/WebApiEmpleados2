
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiEmpleados2.DTOs;
using WebApiEmpleados2.Entidades;

namespace WebApiEmpleados2.Controllers
{
    [ApiController]
    [Route("api/empleados")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class EmpleadosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        //Creacion de Constructores 
        public EmpleadosController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.dbContext = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetEmpleadoDTO>>> Get()
        {
            //Se obtiene una lista del objeto Empleados y se imprimen
            var empelados = await dbContext.Empleados.ToListAsync();
            return mapper.Map<List<GetEmpleadoDTO>>(empelados);
        }
        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<GetEmpleadoDTO>>> Get([FromRoute] string nombre)
        {
            //Se obtiene el objeto Empleados para posteriormente imprimirlo el nombre que ingresa en la peticion
            var empleados = await dbContext.Empleados.Where(empleadoDB => empleadoDB.Nombre.Contains(nombre)).ToListAsync();
            return mapper.Map<List<GetEmpleadoDTO>>(empleados);
        }
        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EmpleadoDTO empleadoDto)
        {
            //Obtenemos el objeto empleadoDto 
            var existeEmpleadoMismoNombre = await dbContext.Empleados.AnyAsync(x => x.Nombre == empleadoDto.Nombre); 
            //Verificamos que no exista el mismo usuario
            if(existeEmpleadoMismoNombre)
            {
                return BadRequest($"Ya existe un empleado con el nombre {empleadoDto.Nombre}");
            }
            //Se realiza el mappeo a empleadoDto del objeto
            var empleado = mapper.Map<Empleado>(empleadoDto);
            //Se guarda el objeto nuevo
            dbContext.Add(empleado);
            //Se guardan los cambios en la DB de manera Asincrona
            await dbContext.SaveChangesAsync();

            var empleadoDTO = mapper.Map<GetEmpleadoDTO>(empleado);
            return CreatedAtRoute("obtenerempleado", new {id = empleado.Id}, empleadoDTO);
        }
        
        [HttpGet("{id:int}", Name = "obtenerempleado")]  //Se puede usar ? para que no sea obligatiorio el paramtero /{param=Alfredo} getEmpleado/{id:int}/
        public async Task<ActionResult<EmpleadoDTOConPuestos>> Get (int id)
        {
            
            
            //Obtenemos el objeto Empleados
            var empleado = await dbContext.Empleados
                //Include realiza que incluya la relacion de EmpleadoPuesto
                .Include(empleadoDB => empleadoDB.EmpleadoPuesto)
                //ThenInclude para obtener el objeto de Puesto
                .ThenInclude(empleadoPuestoDB => empleadoPuestoDB.Puesto)
                .FirstOrDefaultAsync(empleadoBD => empleadoBD.Id == id);

            //Verificamos que el objeto empleado no sea Null
            if(empleado == null)
            {
                return NotFound();
            }
            //Retornamos utilizando el mappeo de EmpleadoDTOConPuestos
            return mapper.Map<EmpleadoDTOConPuestos>(empleado);
        }
        [HttpPut("{id:int}")]// api/empleados/1
        public async Task<ActionResult> Put(EmpleadoDTO empleadoCreacionDTO, int id)
        {
            //Obtenemos del objeto Empleados el id
            var exist = await dbContext.Empleados.AnyAsync(x => x.Id ==id);
            //verificamos que el objeto no sea null
            if(!exist)
            {
                return NotFound();
            }
            //Se realiza el mappeo a la clase EmpleadoCreacionDTO
            var empleado = mapper.Map<Empleado>(empleadoCreacionDTO);
            empleado.Id = id;

            //Se realiza la actualizacion de nuestro DB
            dbContext.Update(empleado);
            //Se guardan los cambios de la DB de manera Asincrona
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            //Obtenemos el id del objeto Empleados
            var exist = await dbContext.Empleados.AnyAsync(x => x.Id == id);
            //Verificamos que no sea null
            if (!exist)
            {
                return NotFound("El recurso no fue encontrado");
            }
            //Se elimina el objeto con el id especificado
            dbContext.Remove(new Empleado()
            {
                Id = id
            });
            //Se guardan los cambio de manera Asincrona
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}