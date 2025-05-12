using API_CAFETAL.Clases;
using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Management;

namespace API_CAFETAL.Controllers
{
    [RoutePrefix("api/Usuario")]
    public class UsuarioController : ApiController
    {
        [HttpGet]
        [Route("ValidarLogin")]

        public bool validarLogin(string correo, string clave)
        {
            clsUsuario Usuario = new clsUsuario();
            return Usuario.validarLogin(correo, clave);
        }

        [HttpGet]
        [Route("ValidarUsuario")]

        public bool validarUsuario(string nombre)
        {
            clsUsuario Usuario = new clsUsuario();
            return Usuario.validarUsuario(nombre);
        }


        [HttpDelete]
        [Route("Eliminar")]

        public string Eliminar(int id) 
        {
            clsUsuario Usuario = new clsUsuario();
           return Usuario.EliminarXId(id);
        }

        [HttpPost]
        [Route("Insertar")]

        public string Insertar([FromBody] USUARIO usuario)
        {
            clsUsuario Usuario = new clsUsuario();

            Usuario.usuario = usuario;

            return Usuario.Insertar();
        }

        [HttpGet]
        [Route("ConsultarTodos")]

        public List<USUARIO> ConsultarTodos()
        {
            clsUsuario Usuario = new clsUsuario();
            return Usuario.ConsultarTodos();
        }

        [HttpGet]
        [Route("Consultar")]

        public USUARIO Consultar(int id)
        {
            clsUsuario Usuario = new clsUsuario();
            return Usuario.Consultar(id);
        }

        [HttpGet]
        [Route("ConsultarCorreo")]

        public bool ConsultarCorreo(string correo) 
        {
            clsUsuario Usuario = new clsUsuario();
            return Usuario.ConsultarCorreo(correo);
        }

        [HttpGet]
        [Route("EstadoUsuario")]
        public IHttpActionResult EstadoUsuario(string correo)
        {
            clsUsuario usuario = new clsUsuario();

            try
            {
                bool estado = usuario.EstadoUsuario(correo); 
                return Ok(estado); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }



        [HttpPut]
        [Route("Actualizar")]

        public string Actualizar([FromBody] USUARIO usuario) 
        {
            clsUsuario Usuario = new clsUsuario();
            Usuario.usuario = usuario;
            return Usuario.Actualizar();

        }

        [HttpGet]
        [Route("ConsultarRol")]
        public int ConsultarRol(string correo)
        {
            clsUsuario Usuario = new clsUsuario();
            return Usuario.ConsultarXCorreo(correo);
        }







    }
}