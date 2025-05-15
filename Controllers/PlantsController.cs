using API_CAFETAL.Clases;
using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        // GET api/<controller>/5
        [HttpPost]
        [Route("Ingreso")]
        public string Ingreso([FromBody] PLANTA planta)
        {
            clsPlanta Planta = new clsPlanta();
            Planta.planta = planta;
            return Planta.Ingreso();
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}