using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_CAFETAL.Clases
{
    public class clsCultivo
    {
        private CAFETALDBEntities dbcafetal = new CAFETALDBEntities();

        public CULTIVO cultivo { get; set; }

        public string Insertar() 
        {
            try
            {
                dbcafetal.CULTIVOes.Add(cultivo);
                dbcafetal.SaveChanges();
                return "Cultivo ingresado con exito";
            }
            catch (Exception ex)
            {
                return "Error al ingresar el cultivo" + ex.Message;
            }

        }
        public List<CULTIVO> ConsultarTodos()
        {
            return dbcafetal.CULTIVOes.ToList();
        }

        public bool ConsultarXId(int id)
        {
            return dbcafetal.CULTIVOes.Any(e => e.id_cultivo == id);
        }
    }


}