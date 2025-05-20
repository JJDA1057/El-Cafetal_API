using API_CAFETAL.Clases;
using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;

namespace API_CAFETAL.Controllers
{
    [System.Web.Http.RoutePrefix("api/adminsumos")]
    public class AdmInsumosController : ApiController
    {
        private readonly clsAdmInsumos _admInsumosManager = new clsAdmInsumos();

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Insertar")]
        public IHttpActionResult InsertarAplicacion([FromBody] ADM_INSUMOS nuevaAplicacion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Validaciones manuales
                if (nuevaAplicacion.id_aplica == 0)
                    return BadRequest("El ID de aplicación es requerido");

                if (!nuevaAplicacion.id_lote.HasValue)
                    return BadRequest("El ID de lote es requerido");

                if (!nuevaAplicacion.id_insumo.HasValue)
                    return BadRequest("El ID de insumo es requerido");

                if (!nuevaAplicacion.cant_usada.HasValue)
                    return BadRequest("La cantidad usada es requerida");

                if (!nuevaAplicacion.fecha_aplic.HasValue)
                    return BadRequest("La fecha de aplicación es requerida");

                if (!nuevaAplicacion.id_proveedor.HasValue)
                    return BadRequest("El ID de proveedor es requerido");

                var resultado = _admInsumosManager.RegistrarAplicacion(
                    nuevaAplicacion.id_aplica,
                    nuevaAplicacion.id_lote.Value,
                    nuevaAplicacion.id_insumo.Value,
                    nuevaAplicacion.cant_usada.Value,
                    nuevaAplicacion.fecha_aplic.Value,
                    nuevaAplicacion.id_proveedor.Value);

                if (resultado.Contains("Error") || resultado.Contains("no existe") || resultado.Contains("insuficiente"))
                {
                    return BadRequest(resultado);
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/adminsumos/ConsultarTodo
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ConsultarTodo")]
        public IHttpActionResult ConsultarTodo()
        {
            try
            {
                var aplicaciones = _admInsumosManager.ConsultarTodas();
                return Ok(aplicaciones);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/adminsumos/ConsultaloteA/5
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ConsultaloteA/{idLote}")]
        public IHttpActionResult ConsultarPorLote(int idLote)
        {
            try
            {
                var aplicaciones = _admInsumosManager.ConsultarPorLote(idLote);
                if (aplicaciones == null || !aplicaciones.Any())
                {
                    return NotFound();
                }
                return Ok(aplicaciones);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/adminsumos/ConsultainsumoA/3
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ConsultainsumoA/{idInsumo}")]
        public IHttpActionResult ConsultarPorInsumo(int idInsumo)
        {
            try
            {
                var aplicaciones = _admInsumosManager.ConsultarPorInsumo(idInsumo);
                if (aplicaciones == null || !aplicaciones.Any())
                {
                    return NotFound();
                }
                return Ok(aplicaciones);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/adminsumos/ConsultafechaA?desde=2023-01-01&hasta=2023-12-31
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ConsultafechaA")]
        public IHttpActionResult ConsultarPorFecha([FromUri] DateTime desde, [FromUri] DateTime hasta)
        {
            try
            {
                var aplicaciones = _admInsumosManager.ConsultarPorFecha(desde, hasta);
                if (aplicaciones == null || !aplicaciones.Any())
                {
                    return NotFound();
                }
                return Ok(aplicaciones);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: api/adminsumos/revertir/5
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("revertir/{idAplica}")]
        public IHttpActionResult RevertirAplicacion(int idAplica)
        {
            try
            {
                var resultado = _admInsumosManager.RevertirAplicacion(idAplica);

                if (resultado.Contains("Error") || resultado.Contains("no encontrado"))
                {
                    return BadRequest(resultado);
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/adminsumos/reportes/masusados
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("reportes/masusados")]
        public IHttpActionResult ReporteInsumosMasUsados()
        {
            try
            {
                var reporte = _admInsumosManager.ObtenerInsumosMasUsados();
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/adminsumos/reportes/consumomensual
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("reportes/consumomensual")]
        public IHttpActionResult ReporteConsumoMensual()
        {
            try
            {
                var reporte = _admInsumosManager.ObtenerConsumoMensual();
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("porLote/{idLote}")]
        public IHttpActionResult ObtenerFertilizantesPorLote(int idLote)
        {
            try 
            {
                var resultado = _admInsumosManager.ConsultarFertilizantesPorLote(idLote);
                    return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }

   
    
}