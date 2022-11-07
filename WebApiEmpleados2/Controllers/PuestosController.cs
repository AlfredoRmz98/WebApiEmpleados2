using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiEmpleados2.DTOs;
using WebApiEmpleados2.Entidades;

namespace WebApiEmpleados2.Controllers
{
    [ApiController]
    [Route("api/puestos")]

    public class PuestosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        //Creacion de Constructores
        public PuestosController(ApplicationDbContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;

        }
        [HttpGet]
        public async Task<ActionResult<List<Puesto>>> GetAll()
        {
            //Se obtiene la lista del objeto Puestos
            return await dbContext.Puestos.ToListAsync();
        }
        
        [HttpPost]
        [ActionName(nameof(PuestoCreacionDTO))]
        public async Task<ActionResult> Post(PuestoCreacionDTO puestoCreacionDTO)
        {
            //Se verifica que el objeto de EmpleadosIds no sea nullo
            if(puestoCreacionDTO.EmpleadosIds == null)
            {
                return BadRequest("No se puede crear una clase sin empleados.");
            }
            //Se obtiene una lista de los empleadosIds del objeto Empleados
            var empleadosIds =await dbContext.Empleados
                .Where(empleadoBD => puestoCreacionDTO.EmpleadosIds.Contains(empleadoBD.Id)).Select(x => x.Id).ToListAsync();
            //Se obtiene del objeto empleadosIds el nombre que exista
            if(puestoCreacionDTO.EmpleadosIds.Count != empleadosIds.Count)
            {
                return BadRequest("No existe uno de los empleados enviados");
            }
            //Se realiza el mappeo de puestoCreacionDTO a Puesto
            var puesto = mapper.Map<Puesto>(puestoCreacionDTO);

            //OrdenarPorEmpleados(puesto);
            //Se añade a la DB
            dbContext.Add(puesto);
            //Se guardan los cambios de nuestra DB de manera asincrona
            await dbContext.SaveChangesAsync();

            //Se realiza el mappeo de puesto a PuestoDTO
            var puestoDTO = mapper.Map<PuestoDTO>(puesto);

            return CreatedAtRoute("obtenerPuesto", new {id = puesto.Id }, puestoDTO);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, PuestoCreacionDTO puestoCreacionDTO)
        {
            //Se obtiene el id del objeto Puestos
            var puestoDB = await dbContext.Puestos
                //Se incluye la relacion de EmpleadoPuesto
                .Include(x => x.EmpleadoPuesto)
                .FirstOrDefaultAsync(x => x.Id == id);

            //Verifica que no sea null
            if (puestoDB == null)
            {
                return NotFound();
            }
            
            //Se realiza el mappeo de puesto DB a PuestoCracionDTO
            puestoDB = mapper.Map(puestoCreacionDTO, puestoDB);

            //OrdenarPorAlumnos(puestoDB);
            //Se guardan los cambios de nuestra DB de manera asincrona
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            //Se obtiene del id del objeto Puestos
            var exist = await dbContext.Puestos.AnyAsync(x => x.Id == id);
            //Verifica si no existe 
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado.");
            }
            //Se realiza la eliminacion en nuestra DB
            dbContext.Remove(new Puesto { Id = id });
            //Se guardan los cambios de nuestra DB de manera asíncrona
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}