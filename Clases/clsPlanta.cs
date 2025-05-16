
using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_CAFETAL.Clases
{
    public class clsPlanta
    {
        public PLANTA planta { get; set; }
        private CAFETALDBEntities dbcafetal = new CAFETALDBEntities();



        public string Ingreso()
        {

           
            try
            {
                dbcafetal.PLANTAs.Add(planta);
                dbcafetal.SaveChanges();
                return "Planta ingresada con exito";
            }
            catch (Exception ex)
            {
                return "Nos topamos con un error al Ingresar la Planta" + ex.Message;
            }
        }

        public List<PLANTA> ConsultarTodos()
        {
            return dbcafetal.PLANTAs.ToList();
        }

        public async Task<string> IngresoD(int lote, DateTime Fecha_siembra, string estado, string observaciones)
        {
            try
            {
                PLANTA planta = new PLANTA();
                planta.id_lote = lote;
                planta.fecha_plantacion = Fecha_siembra;
                planta.estado = estado;
                planta.observaciones = observaciones;

                dbcafetal.PLANTAs.Add(planta);
                await dbcafetal.SaveChangesAsync();


                return "Su planta fue ingresada en el lote con exito ";
            }
            catch (Exception ex)
            {
                return "Su planta no pudo ser registrada " + ex.Message;
            }
        }
        public string ModificarEstado(int id_planta, string estado)
        {
            try
            {
                var Planta_ingresada = dbcafetal.PLANTAs.Find(id_planta);
                if (Planta_ingresada == null)
                    return "Planta no encontrada con el ID diligenciado";

                Planta_ingresada.estado = estado;
                dbcafetal.SaveChanges();

                return "Estado Restaurado";
            }
            catch (Exception ex)
            {
                return "No pudimos modificar el estado de su planta" + ex.Message;
            }



        }

        public List<PLANTA> consultarXLote(int lote)
        {
            return dbcafetal.PLANTAs.Where(e => e.id_lote == lote).ToList();
        }

        public string ModificarObservaciones(int id_planta, string observacion)
        {
            try
            {
                var planta_ingresada = dbcafetal.PLANTAs.Find(id_planta);
                if (planta_ingresada == null)
                    return "No se encontre esta planta con este ID";

                planta_ingresada.observaciones= observacion;
                dbcafetal.SaveChanges();

                return "Sus observaciones fueron modificadas con exito";
            }
            catch (Exception ex)
            {
                return "Sus observaciones fueron denegadas..."+ex.Message;
            }
        }
    }
}