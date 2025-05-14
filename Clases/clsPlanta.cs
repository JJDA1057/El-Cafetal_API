using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_CAFETAL.Clases
{
    public class clsPlanta
    {

        public Planta planta { get; set; }

        private CAFETALDBEntities dbcafetal = new CAFETALDBEntities();


        public string Ingreso()
        {
            try
            {
                dbcafetal.Plantas.Add(planta);
                dbcafetal.SaveChanges();
                return "Planta ingresada con exito";
            }
            catch (Exception ex)
            {
                return "Nos topamos con un error al Ingresar la Planta" + ex.Message;
            }
        }

        public List<Planta> ConsultarTodos()
        {
            return dbcafetal.Plantas.ToList();
        }
    }
}