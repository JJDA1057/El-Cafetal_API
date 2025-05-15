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
    [RoutePrefix("api/Reportes")]
    public class ReporteController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        [Route("api/ConsultarTodos")]
        public List<REPORTE> ConsultarTodos()
        {
            clsReporte reporte = new clsReporte();
            return reporte.ConsultarTodos();
        }
        // POST api/<controller>
        [HttpPost]
        [Route("api/Reportes/InsertarReporte")]
        public IHttpActionResult InsertarReporte([FromBody] REPORTE reporte)
        {
            clsReporte consulta = new clsReporte();
            string resultado = consulta.InsertarReporte(reporte);
            return Ok(resultado);
        }
        // GET api/<controller>/5
        [HttpGet]
        [Route("api/ConsultarPorTipoProblema")]
        public List<REPORTE> ConsultarXTipoProblema(string problema)
        {
            clsReporte reporte = new clsReporte();
            return reporte.ConsultarXTipoProblema(problema);
        }
        // PUT api/<controller>/5
        [HttpPut]
        [Route("api/ActualizarReporte/{id}")]
        public string ActualizarReporte(int id, [FromBody] REPORTE reporte)
        {
            if (id != reporte.id_report)
                return "Este ID no coincide.";

            clsReporte gestor = new clsReporte();
            return gestor.ActualizarReporte(reporte);
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        [Route("api/EliminarReporte/{id}")]
        public string EliminarReporte(int id)
        {
            clsReporte reporte = new clsReporte();
            return reporte.EliminarReporte(id);
        }
        public void Delete(int id)
        {
        }
    }
}