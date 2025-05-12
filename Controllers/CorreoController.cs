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
    [RoutePrefix("api/Correo")]
    public class CorreoController : ApiController
    {
       
        private CAFETALDBEntities dbcafetal = new CAFETALDBEntities();
        [HttpPost]
        [Route("EnviarCorreo")]

        public async Task<IHttpActionResult> EnviarCorreoVerificacion([FromBody] string correo)
        {
            try
            {
                

                var codigoVerificacion = new Random().Next(100000, 999999).ToString();

                var usuario = dbcafetal.USUARIOs.FirstOrDefault(u => u.correo == correo);
                if (usuario == null)
                {
                    return BadRequest("Usuario no encontrado.");
                }

              
                usuario.codigo_verificacion = codigoVerificacion;
                dbcafetal.SaveChanges();



                clsCorreo correoService = new clsCorreo();
                bool correoEnviado = await correoService.EnviarCorreoVerificacionAsync(correo, codigoVerificacion);

                if (correoEnviado)
                {
                    return Ok("Correo de verificación enviado.");
                }
                else
                {
                    return InternalServerError(new Exception("Error al enviar el correo de verificación."));
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



        [HttpPost]
        [Route("VerificarCorreo")]
        public IHttpActionResult VerificarCorreo(string correo, string codigoVerificacion)
        {
            var usuario = dbcafetal.USUARIOs.FirstOrDefault(u => u.correo == correo);
            if (usuario == null)
            {
                return BadRequest("Usuario no encontrado.");
            }

            if (usuario.codigo_verificacion == codigoVerificacion)
            {
                usuario.estado_verificacion = true;
                dbcafetal.SaveChanges();
                return Ok("Correo verificado exitosamente.");
            }
            else
            {
                return BadRequest("Código de verificación incorrecto.");
            }
        }
    }
}