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
    [RoutePrefix("api/Cultivos")]
    public class CultivosController : ApiController
    {

        [HttpGet]
        [Route("ConsultarTodos")]

        public List<CULTIVO> ConsultarTodos()
        {
            clsCultivo Cultivo = new clsCultivo();
            return Cultivo.ConsultarTodos();
        }

        [HttpPost]
        [Route("Insertar")]

        public string Insertar([FromBody] CULTIVO cultivo)
        {
            clsCultivo Cultivo = new clsCultivo();

            Cultivo.cultivo = cultivo;

            return Cultivo.Insertar();
        }


        [HttpGet]
        [Route("ConsultarXId")]

        public bool ConsultarXId(int id) 
        {
            clsCultivo cultivo = new clsCultivo();
            return cultivo.ConsultarXId(id);
        }
    }
}