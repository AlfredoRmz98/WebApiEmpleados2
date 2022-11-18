using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiEmpleados2.DTOs;
using WebApiEmpleados2.Entidades;

namespace WebApiEmpleados2.Controllers
{
    [ApiController]
    [Route("puestos/{puestoId:int}/departamentos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "esEmpleado")]
    
    public class DepartamentosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        //Creacion de Constructores
        public DepartamentosController(ApplicationDbContext dbContext, IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<DepartamentoDTO>>> Get(int puestoId)
        {
            //Obtenemos del objeto Puesto el id
            var existePuesto = await dbContext.Puestos.AnyAsync(puestoDB => puestoDB.Id == puestoId);
            //Verificamos que exista el id
            if (!existePuesto)
            {
                return NotFound();
            }
            //Se obtiene del objeto Departamentos el id 
            var departamentos = await dbContext.Departamentos.Where(departamentoDB => departamentoDB.PuestoId == puestoId).ToListAsync();
            //Se crea y retorna la lista del mappeo de DepartamentoDTO
            return mapper.Map<List<DepartamentoDTO>>(departamentos);
        }

        [HttpGet("{id:int}", Name = "obtenerDepartamento")]
        public async Task<ActionResult<DepartamentoDTO>> GetById(int id)
        {
            //Se obtiene el id del objeto Departamentos
            var departamento = await dbContext.Departamentos.FirstOrDefaultAsync(departamentoDB => departamentoDB.Id == id);
            //Se verifica si departamento es null o no
            if (departamento == null)
            {
                return NotFound();
            }
            //Se retorna el mappeo de DepartamentoDTO
            return mapper.Map<DepartamentoDTO>(departamento);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Post(int puestoId, DepartamentoCreacionDTO departamentoCreacionDTO)
        {
           
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = emailClaim.Value;
            //Se obtiene del objeto userManager el email
            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioId = usuario.Id;
            //Se obtiene del objeto Departamentos el id del puesto
            var existeDepartamento = await dbContext.Departamentos.AnyAsync(puestoDB => puestoDB.Id == puestoId);
            //Verificamos si no existe existeDEpartamentos
            if (!existeDepartamento)
            {
                return NotFound("Se guardo correctamente");
            }
            //Se mapea departamentoCreacionDTO
            var departamento = mapper.Map<Departamentos>(departamentoCreacionDTO);
            departamento.PuestoId = puestoId;
            departamento.UsuarioId = usuarioId;
            //Se agraga a nuestra DB
            dbContext.Add(departamento);
            //Se guarda los cambios de nuestra DB de manera Asincrona
            await dbContext.SaveChangesAsync();
            //Se mappea departamentoDTO
            var departamentoDTO = mapper.Map<DepartamentoDTO>(departamento);

            return CreatedAtRoute("obtenerDepartamento", new { id = departamento.Id, puestoId = puestoId }, departamentoDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int puestoId, int id, DepartamentoCreacionDTO departamentoCreacionDTO)
        {
            //Se obtiene del objeto Departamentos el id puesto
            var existePuesto = await dbContext.Departamentos.AnyAsync(puestoDB => puestoDB.Id == puestoId);
            //Verificamos que exista
            if (!existePuesto)
            {
                return NotFound();
            }
            //Se obtiene del objeto Departamentos el id
            var existeDepartamento = await dbContext.Departamentos.AnyAsync(departamentoDB => departamentoDB.Id == id);
            //Verificamos que exista
            if (!existeDepartamento)
            {
                return NotFound();
            }

            //Se realiza el mappeo de departamentoCreacionDTO y se cargan el id y puestoid
            var puesto = mapper.Map<Departamentos>(departamentoCreacionDTO);
            puesto.Id = id;
            puesto.PuestoId = puestoId;
            //Se actualiza la DB
            dbContext.Update(puesto);
            //Se guardan los cambios de nuestra DB de manera asincrona
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}