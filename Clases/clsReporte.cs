using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_CAFETAL.Clases
{
    public class clsReporte
    {
        public REPORTE reporte { get; set; }
        private CAFETALDBEntities dbcafetal = new CAFETALDBEntities();

        public List<REPORTE> ConsultarTodos()
        {
            return dbcafetal.REPORTES.ToList();
        }

        public string InsertarReporte(REPORTE reporte)
        {
            try
            {
                var dbCafetal = new CAFETALDBEntities();
                dbCafetal.REPORTES.Add(reporte);
                dbCafetal.SaveChanges();
                return "Reporte generado con éxito";
            }
            catch (Exception ex)
            {
                return "Error al ingresar el reporte" + ex.Message;
            }
        }
        public List<REPORTE> ConsultarXTipoProblema(string problema)
        {
            var dbCafetal = new CAFETALDBEntities();
            return dbCafetal.REPORTES.Where(r =>problema== problema).ToList();
        }

        public string ActualizarReporte(REPORTE reporte)
        {
            try
            {
                var dbCafetal = new CAFETALDBEntities();
                var reporteExistente = dbCafetal.REPORTES.Find(reporte.id_report);

                if (reporteExistente == null)
                    return "Reporte no encontrado";

                reporteExistente.comentarios = reporte.comentarios;
                reporteExistente.fecha= reporte.fecha;
                reporteExistente.id_report = reporte.id_report;
                reporteExistente.id_lote = reporte.id_lote;
                

                dbCafetal.SaveChanges();
                return "Reporte actualizado con éxito";
            }
            catch (Exception ex)
            {
                return "Error al actualizar el reporte: " + ex.Message;
            }
        }
        public string EliminarReporte(int id_report)
        {
            try
            {
                var dbCafetal = new CAFETALDBEntities();
                var reporte = dbCafetal.REPORTES.Find(id_report);

                if (reporte == null)
                    return "Reporte no encontrado";

                dbCafetal.REPORTES.Remove(reporte);
                dbCafetal.SaveChanges();
                return "Reporte eliminado con éxito";
            }
            catch (Exception ex)
            {
                return "Error al eliminar el reporte: " + ex.Message;
            }
        }



    }
}