using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http.Results;


namespace API_CAFETAL.Clases
{
    public class clsUsuario
    {
        private CAFETALDBEntities dbcafetal = new CAFETALDBEntities();

        public USUARIO usuario { get; set; }

        public bool validarLogin(string correo, string clave)
        {
            return dbcafetal.USUARIOs.Any(e => e.correo == correo && e.contraseña == clave);
        }

        public bool validarUsuario(string nombre)
        {
            return dbcafetal.USUARIOs.Any(e => e.nombre == nombre);
        }

        public string Insertar()
        {
            try
            {
                dbcafetal.USUARIOs.Add(usuario);
                dbcafetal.SaveChanges();
                return "Usuario ingresado con exito";
            }
            catch (Exception ex)
            {
                return "Error al ingresar el usuario" + ex.Message;
            }
        }


        public List<USUARIO> ConsultarTodos()
        {
            return dbcafetal.USUARIOs.ToList();
        }

        public USUARIO Consultar(int id)
        {
            USUARIO us = dbcafetal.USUARIOs.FirstOrDefault(e => e.id_usuario == id);
            return us;
        }


        public bool ConsultarCorreo(string correo)
        {
            return dbcafetal.USUARIOs.Any(e => e.correo == correo);
        }

        public bool EstadoUsuario(string correo)
        {
            var usuario = dbcafetal.USUARIOs.FirstOrDefault(e => e.correo == correo);

            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado");


            }
            return usuario.estado_usuario;
        }


        public string Actualizar()
        {
            USUARIO usu = Consultar(usuario.id_usuario);
            if (usu == null)
            {
                return "Id no valido";
            }
            dbcafetal.USUARIOs.AddOrUpdate(usuario);
            dbcafetal.SaveChanges();
            return "Se actualizo el registro correctamente";
        }

        public string EliminarXId(int id)
        {
            try
            {
                USUARIO us = Consultar(id);
                if (us == null)
                {
                    return "Id no valido";
                }
                dbcafetal.USUARIOs.Remove(us);
                dbcafetal.SaveChanges();
                return "Usuario eliminado correctamente";
            }
            catch (Exception ex) 
            {
                return ex.Message;
            }
        }

        public int ConsultarXCorreo(string correo) 
        {
            USUARIO us = dbcafetal.USUARIOs.FirstOrDefault(e => e.correo == correo);
            if (us != null)
            {
                return us.id_rol;
            }
            else
            {
                return -1; 
            }
        }
    } 
}