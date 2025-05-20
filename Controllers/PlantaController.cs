using API_CAFETAL.Clases;
using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace API_CAFETAL.Controllers
{
    [System.Web.Http.RoutePrefix("api/Plantas")]
    public class PlantaController : ApiController
    {
        private readonly clsPlanta _plantaManager;

        public PlantaController()
        {
            _plantaManager = new clsPlanta();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ConsultarTodos")]
        public IHttpActionResult ObtenerTodasLasPlantas()
        {
            try
            {
                var plantas = _plantaManager.ConsultarTodas();
                return Ok(plantas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ConsultarPorLote/{idLote}")]
        public IHttpActionResult ObtenerPlantasPorLote(int idLote)
        {
            try
            {
                var plantas = _plantaManager.ConsultarPorLote(idLote);
                return Ok(plantas);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("Consultar/{id}")]
        public IHttpActionResult ObtenerPorId(int id)
        {
            try
            {
                var plant = _plantaManager.ConsultarPorId(id);
                if (plant == null)
                {
                    return NotFound();
                }
                return Ok(plant);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Registrar")]
        public IHttpActionResult RegistrarNuevaPlanta([FromBody] PLANTA planta)
        {
            try
            {
                var resultado = _plantaManager.RegistrarPlanta(
                    planta.id_planta,
                    planta.id_lote,
                    planta.estado,
                    planta.observaciones,
                    planta.fecha_plantacion);

                if (resultado.StartsWith("Error"))
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("Borrar/{id}")]
        public IHttpActionResult BorrarPlanta(int idPlanta)
        {
            try
            {
                var resultado = _plantaManager.Borrar(idPlanta);

                if (resultado.StartsWith("Error") || resultado.Contains("no encontrada"))
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("ModificarEstado/{idPlanta}")]
        public IHttpActionResult ActualizarEstadoPlanta(int idPlanta, [FromBody] string estado)
        {
            try
            {
                var resultado = _plantaManager.ModificarEstado(idPlanta, estado);

                if (resultado.StartsWith("Error") || resultado.StartsWith("Planta no encontrada"))
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("ModificarObservaciones/{idPlanta}")]
        public IHttpActionResult ActualizarObservacionesPlanta(int idPlanta, [FromBody] string observaciones)
        {
            try
            {
                var resultado = _plantaManager.ModificarObservaciones(idPlanta, observaciones);

                if (resultado.StartsWith("Error") || resultado.StartsWith("Planta no encontrada"))
                    return BadRequest(resultado);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}