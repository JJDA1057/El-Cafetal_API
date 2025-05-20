
using API_CAFETAL.Clases;
using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;

namespace API_CAFETAL.Controllers
{
    [System.Web.Http.RoutePrefix("api/Insumo")]
    public class InsumoController : ApiController
    {
        private readonly clsInsumo _insumoManager = new clsInsumo();

        // GET: api/insumos
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ConsultarTodos")]
        public IHttpActionResult ObtenerTodos()
        {
            try
            {
                var insumos = _insumoManager.ConsultarTodos();
                return Ok(insumos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/insumos/5
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("Consultar/{id}")]
        public IHttpActionResult ObtenerPorId(int id)
        {
            try
            {
                var insumo = _insumoManager.ConsultarPorId(id);
                if (insumo == null)
                {
                    return NotFound();
                }
                return Ok(insumo);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/insumos/tipo/fertilizante
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("tipo/{tipo}")]
        public IHttpActionResult ObtenerPorTipo(string tipo)
        {
            try
            {
                var insumos = _insumoManager.ConsultarPorTipo(tipo);
                if (insumos == null || !insumos.Any())
                {
                    return NotFound();
                }
                return Ok(insumos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/insumos/proveedor/3
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ConsultarProveedor/{idProveedor}")]
        public IHttpActionResult ObtenerPorProveedor(int idProveedor)
        {
            try
            {
                var insumos = _insumoManager.ConsultarPorProveedor(idProveedor);
                if (insumos == null || !insumos.Any())
                {
                    return NotFound();
                }
                return Ok(insumos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/insumos
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Registrar")]
        public IHttpActionResult RegistrarInsumo([FromBody] INSUMO nuevoInsumo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Validar que id_proveedor no sea nulo
                if (!nuevoInsumo.id_proveedor.HasValue)
                {
                    return BadRequest("El ID del proveedor es requerido");
                }

                // Validar que cantidad no sea nula
                if (!nuevoInsumo.cantidad.HasValue)
                {
                    return BadRequest("La cantidad es requerida");
                }

                var resultado = _insumoManager.RegistrarInsumo(
                    nuevoInsumo.id_insumo,
                    nuevoInsumo.nombre,
                    nuevoInsumo.tipo,
                    nuevoInsumo.id_proveedor.Value, // Usamos .Value para obtener el int
                    nuevoInsumo.cantidad.Value,
                    nuevoInsumo.f_entrega.Value);

                if (resultado.Contains("Error") || resultado.Contains("existe"))
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

        // PUT: api/insumos/5
        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("Actualizar/{id}")]
        public IHttpActionResult ActualizarInsumo(int id, [FromBody] INSUMO insumoActualizado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != insumoActualizado.id_insumo)
                {
                    return BadRequest("ID del insumo no coincide");
                }

                var resultado = _insumoManager.ActualizarInsumo(
                    id,
                    insumoActualizado.nombre,
                    insumoActualizado.tipo,
                    insumoActualizado.id_proveedor.Value,
                    insumoActualizado.cantidad.Value,
                    insumoActualizado.f_entrega.Value);

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

        // DELETE: api/insumos/5
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("Eliminar/{id}")]
        public IHttpActionResult EliminarInsumo(int id)
        {
            try
            {
                var resultado = _insumoManager.EliminarInsumo(id);

                if (resultado.Contains("Error") || resultado.Contains("no encontrado") || resultado.Contains("asociados"))
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

        // PATCH: api/insumos/5/cantidad
        [System.Web.Http.HttpPatch]
        [System.Web.Http.Route("{id}/cantidad")]
        public IHttpActionResult ActualizarCantidad(int id, [FromBody] ActualizacionCantidadModel model)
        {
            try
            {
                var resultado = _insumoManager.ActualizarCantidad(
                    id,
                    model.Cantidad,
                    model.EsSuma);

                if (resultado.Contains("Error") || resultado.Contains("no encontrado") || resultado.Contains("suficiente"))
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
    


    //------------------------------------>Observer

    
    [System.Web.Http.HttpGet]
        [System.Web.Http.Route("nivelesBajos")]
        public IHttpActionResult ObtenerNivelesBajos()
        {
            try
            {
                var resultado = _insumoManager.ConsultarNivelesBajos();

                if (resultado == null || resultado.Count == 0)
                {
                    return NotFound();
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
            return InternalServerError(ex);
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("nivelesCriticos")]
        public IHttpActionResult ObtenerNivelesCriticos()
        {
            try
            {
                var resultado = _insumoManager.ConsultarNivelesCriticos();

                if (resultado == null || resultado.Count == 0)
                {
                    return NotFound();
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("verificar-nivel/{id}")]
        public IHttpActionResult VerificarNivel(int id)
        {
            try
            {
                var resultado = _insumoManager.VerificarNivelPorId(id);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //[System.Web.Http.HttpPost]
        //[System.Web.Http.Route("configurar-umbrales")]
        //public IHttpActionResult ConfigurarUmbral([FromBody] UmbralRequest model)
        //{
        //    try
        //    {
        //        _insumoManager.ConfigurarUmbral(model.Tipo, model.UmbralBajo, model.UmbralCritico);

        //        return Ok($"Umbral para {model.Tipo} actualizado correctamente.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}

        //public class UmbralRequest
        //{
        //    public string Tipo { get; set; }
        //    public int UmbralBajo { get; set; }
        //    public int UmbralCritico { get; set; }
        //}








    }
    // Modelo auxiliar para actualización de cantidad
    public class ActualizacionCantidadModel
    {
        public int Cantidad { get; set; }
        public bool EsSuma { get; set; }
    };
}
