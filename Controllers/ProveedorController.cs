using API_CAFETAL.Clases;
using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace API_CAFETAL.Controllers
{
    [RoutePrefix("api/Proveedores")]
    public class ProveedorController : ApiController
    {
        private readonly clsProveedor _proveedorManager = new clsProveedor();

        // GET: api/proveedores
        [HttpGet]
        [Route("Consultar")]
        public IHttpActionResult ObtenerTodos()
        {
            try
            {
                var proveedores = _proveedorManager.ConsultarTodos();
                return Ok(proveedores);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/proveedores/5
        [HttpGet]
        [Route("Consultar/{id}")]
        public IHttpActionResult ObtenerPorId(int id)
        {
            try
            {
                var proveedor = _proveedorManager.ConsultarPorId(id);
                if (proveedor == null)
                {
                    return NotFound();
                }
                return Ok(proveedor);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/proveedores/material/{tipoMaterial}
        [HttpGet]
        [Route("Material/{tipoMaterial}")]
        public IHttpActionResult ObtenerPorTipoMaterial(string tipoMaterial)
        {
            try
            {
                var proveedores = _proveedorManager.ConsultarPorTipoMaterial(tipoMaterial);
                if (proveedores == null || !proveedores.Any())
                {
                    return NotFound();
                }
                return Ok(proveedores);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/proveedores
        [HttpPost]
        [Route("Registrar")]
        public IHttpActionResult RegistrarProveedor([FromBody] PROVEEDOR proveedor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var resultado = _proveedorManager.RegistrarProveedor(
                    proveedor.id_proveedor,
                    proveedor.nombre,
                    proveedor.cel_fijo,
                    proveedor.tipo_material);

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

        // PUT: api/proveedores/5
        [HttpPut]
        [Route("Actualizar/{id}")]
        public IHttpActionResult ActualizarProveedor(int id, [FromBody] PROVEEDOR proveedor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != proveedor.id_proveedor)
                {
                    return BadRequest("ID del proveedor no coincide");
                }

                var resultado = _proveedorManager.ActualizarProveedor(
                    id,
                    proveedor.nombre,
                    proveedor.cel_fijo,
                    proveedor.tipo_material);

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

        // DELETE: api/proveedores/5
        [HttpDelete]
        [Route("Borrar/{id}")]
        public IHttpActionResult EliminarProveedor(int id)
        {
            try
            {
                var resultado = _proveedorManager.EliminarProveedor(id);

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
    }
}