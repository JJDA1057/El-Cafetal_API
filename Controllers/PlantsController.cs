using API_CAFETAL.Clases;
using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace API_CAFETAL.Controllers
{
    [RoutePrefix("api/Plantas")]
    public class PlantsController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        [Route("ConsultarTodos")]
        public List<PLANTA> ConsultarTodos()
        {
            clsPlanta planta = new clsPlanta();
            return planta.ConsultarTodos();

        }

        [HttpPost]
        [Route("Ingreso")]
        public string Ingreso([FromBody] PLANTA planta)
        {
            clsPlanta Planta = new clsPlanta();
            Planta.planta = planta;
            return Planta.Ingreso();
        }

        [HttpGet]
        [Route("ConsultarPorLote")]

        public List<PLANTA> consultarXLote(int lote)
        {
            clsPlanta planta = new clsPlanta();
            return planta.consultarXLote(lote);
        }

        // POST api/<controller>

        [HttpPost]
        [Route("IngresoDePlantas")]
        public async Task<string> IngresoD(int lote, DateTime Fecha_siembra, string estado, string observaciones)
        {
            clsPlanta planta = new clsPlanta();
            planta.planta.id_lote = lote;   
            return await planta.IngresoD(lote, Fecha_siembra, estado, observaciones);
        }
        [HttpPost]
        [Route("ModificarEstadoPlantas")]
        
        public string ModificarEstado([FromBody]int id, string estado)
        {
            clsPlanta planta = new clsPlanta();
            return planta.ModificarEstado(id, estado);
        }

        // POST api/<controller>
        [HttpPost]
        [Route("ModificarObservacionPlantas")]
        public string ModificarObservacion([FromBody] int id, string observacion)
        {
            clsPlanta planta = new clsPlanta();
            return planta.ModificarObservaciones(id, observacion);
        }
      
    }
}