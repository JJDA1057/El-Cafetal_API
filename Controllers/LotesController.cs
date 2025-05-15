    using API_CAFETAL.Clases;
using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_CAFETAL.Controllers
{
    [RoutePrefix("api/Lotes")]
    public class LoteController : ApiController
    {
        private readonly clsLote _loteManager = new clsLote();

        // POST: api/lotes/insertar
        [HttpPost]
        [Route("Registrar")]
        public IHttpActionResult InsertarLote([FromBody] LOTE nuevoLote)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Validaciones manuales
                if (nuevoLote.id_usuario == 0)
                    return BadRequest("El ID de usuario es requerido");

                if (string.IsNullOrWhiteSpace(nuevoLote.variedad))
                    return BadRequest("La variedad es requerida");

                if (nuevoLote.area <= 0)
                    return BadRequest("El área debe ser mayor a cero");

                if (nuevoLote.id_cultivo == 0)
                    return BadRequest("El ID de cultivo es requerido");

                var resultado = _loteManager.RegistrarLote(
                    nuevoLote.id_lote,
                    nuevoLote.id_usuario,
                    nuevoLote.variedad,
                    nuevoLote.inicio_siembra.Value,
                    nuevoLote.estado,
                    nuevoLote.fecha_fin_siembra,
                    nuevoLote.area.Value,
                    nuevoLote.cantidad_plantas.Value,
                    nuevoLote.observaciones,
                    nuevoLote.id_cultivo.Value);

                if (resultado.Contains("Error"))
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

        // PUT: api/lotes/actualizar/5
        [HttpPut]
        [Route("actualizar/{id}")]
        public IHttpActionResult ActualizarLote(int id, [FromBody] LOTE loteActualizado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != loteActualizado.id_lote)
                    return BadRequest("ID de lote no coincide");

                var resultado = _loteManager.ActualizarLote(
                    loteActualizado.id_lote,
                    loteActualizado.id_usuario,
                    loteActualizado.variedad,
                    loteActualizado.inicio_siembra.Value,
                    loteActualizado.estado,
                    loteActualizado.fecha_fin_siembra,
                    loteActualizado.area.Value,
                    loteActualizado.cantidad_plantas.Value,
                    loteActualizado.observaciones,
                    loteActualizado.id_cultivo.Value);

                if (resultado.Contains("Error"))
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

        // GET: api/lotes
        [HttpGet]
        [Route("Consultar")]
        public IHttpActionResult ObtenerTodos()
        {
            try
            {
                var lotes = _loteManager.ConsultarTodos();
                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/lotes/5
        [HttpGet]
        [Route("Consultar/{id}")]
        public IHttpActionResult ObtenerPorId(int id)
        {
            try
            {
                var lote = _loteManager.ConsultarPorId(id);
                if (lote == null)
                {
                    return NotFound();
                }
                return Ok(lote);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/lotes/estado/activo
        [HttpGet]
        [Route("estado/{estado}")]
        public IHttpActionResult ObtenerPorEstado(string estado)
        {
            try
            {
                var lotes = _loteManager.ConsultarPorEstado(estado);
                if (lotes == null || !lotes.Any())
                {
                    return NotFound();
                }
                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/lotes/cultivo/3
        [HttpGet]
        [Route("cultivo/{idCultivo}")]
        public IHttpActionResult ObtenerPorCultivo(int idCultivo)
        {
            try
            {
                var lotes = _loteManager.ConsultarPorCultivo(idCultivo);
                if (lotes == null || !lotes.Any())
                {
                    return NotFound();
                }
                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PATCH: api/lotes/cambiar-estado/5
        [HttpPatch]
        [Route("cambiar-estado/{id}")]
        public IHttpActionResult CambiarEstado(int id, [FromBody] CambioEstadoModel cambio)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var resultado = _loteManager.CambiarEstado(id, cambio.NuevoEstado, cambio.FechaFin);
                if (resultado.Contains("Error"))
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

        // GET: api/lotes/resumen-variedades
        [HttpGet]
        [Route("resumen")]
        public IHttpActionResult ObtenerResumenVariedades()
        {
            try
            {
                var resumen = _loteManager.ObtenerResumenPorVariedad();
                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/lotes/proximos-cosecha
        [HttpGet]
        [Route("proxima-cosecha")]
        public IHttpActionResult ObtenerProximosACosecha()
        {
            try
            {
                var lotes = _loteManager.ObtenerLotesProximosACosechar();
                return Ok(lotes);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/lotes/productividad
        [HttpGet]
        [Route("productividad")]
        public IHttpActionResult ObtenerProductividad()
        {
            try
            {
                var reporte = _loteManager.ObtenerProductividad();
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }

    // Modelo solo para cambio de estado
    public class CambioEstadoModel
    {
        [Required]
        public string NuevoEstado { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}