using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practicando_labo1_daw.Models;

namespace practicando_labo1_daw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public equiposController(equiposContext equiposContext)
        {
            _equiposContexto = equiposContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult getAll()
        {
            List<equipos> equiposLista = (from e in _equiposContexto.equipos select e).ToList();

            if (equiposLista.Count() == 0)
            {
                return NotFound();
            }
            return Ok(equiposLista);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult getById(int id)
        {
            equipos? equiposData = (from e in _equiposContexto.equipos where e.id_equipos == id select e).FirstOrDefault();

            if (equiposData == null)
            {
                return NotFound();
            }

            return Ok(equiposData);
        }

        [HttpGet]
        [Route("Find/{filter}")]
        public IActionResult findByDescription(String filter)
        {
            equipos? equiposByFilter = (from e in _equiposContexto.equipos where e.descripcion.Contains(filter) select e).FirstOrDefault();
            if(equiposByFilter == null)
            {
                return NotFound();
            }
            return Ok(equiposByFilter);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult guardarRegistro([FromBody] equipos equipo)
        {
            try
            {
                _equiposContexto.equipos.Add(equipo);
                _equiposContexto.SaveChanges();
                return Ok(equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult updateBD(int id, [FromBody] equipos modificarEquipos)
        {
            equipos? equiposData = (from e in _equiposContexto.equipos where e.id_equipos == id select e).FirstOrDefault();

            if (equiposData == null)
            {
                return NotFound();
            }

            equiposData.nombre = modificarEquipos.nombre;
            equiposData.descripcion = modificarEquipos.descripcion;
            equiposData.tipo_equipo_id = modificarEquipos.tipo_equipo_id;
            equiposData.marca_id = modificarEquipos.marca_id;
            equiposData.anio_compra = modificarEquipos.anio_compra;
            equiposData.costo = modificarEquipos.costo;

            _equiposContexto.Entry(equiposData).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(modificarEquipos);

        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult delete(int id)
        {
            equipos? equiposData = (from e in _equiposContexto.equipos where e.id_equipos == id select e).FirstOrDefault();
            if (equiposData == null)
            {
                return NotFound();
            }
            _equiposContexto.equipos.Attach(equiposData);
            _equiposContexto.equipos.Remove(equiposData);
            _equiposContexto.SaveChanges();

            return Ok(equiposData);
        }
    }
}
