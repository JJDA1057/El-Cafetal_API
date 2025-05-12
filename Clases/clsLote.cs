using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_CAFETAL.Clases
{
    public class clsLote
    {
        private CAFETALDBEntities dbcafetal = new CAFETALDBEntities();

        public LOTE lote { get; set; }

        public string Insertar()
        {
            try
            {
                dbcafetal.LOTEs.Add(lote);
                dbcafetal.SaveChanges();
                return "Lote ingresado con exito";
            }
            catch (Exception ex)
            {
                return "Error al ingresar el lote" + ex.Message;
            }

        }

        public List<LOTE> ConsultarTodos()
        {
            return dbcafetal.LOTEs.ToList();
        }
    }
}