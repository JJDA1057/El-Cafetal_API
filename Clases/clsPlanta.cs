
using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_CAFETAL.Clases
{
    public class clsPlanta
    {
        private CAFETALDBEntities dbcafetal = new CAFETALDBEntities();



        public string RegistrarPlanta(int idPlanta, int idLote, string estado, string observaciones, DateTime fechaPlantacion)
        {
            try
            {
                if (!LoteExiste(idLote))
                    return "El lote no existe";

                var nuevaPlanta = new PLANTA
                {
                    id_planta = idPlanta,
                    id_lote = idLote,
                    estado = estado,
                    observaciones = observaciones,
                    fecha_plantacion = fechaPlantacion
                };

                dbcafetal.PLANTAs.Add(nuevaPlanta);
                dbcafetal.SaveChanges();
                return "Planta registrada correctamente";
            }
            catch (Exception ex)
            {
                return "Error al registrar planta: " + ex.Message;
            }
        }

        public PLANTA ConsultarPorId(int idPlanta)
        {
            return dbcafetal.PLANTAs.FirstOrDefault(p => p.id_planta == idPlanta);
                
        }

        public string Borrar(int idPlanta)
        {
            try
            {
                // Buscar la planta por ID
                var planta = dbcafetal.PLANTAs.Find(idPlanta);

                // Verificar si existe
                if (planta == null)
                    return "Planta no encontrada";

                // Eliminar la planta
                dbcafetal.PLANTAs.Remove(planta);
                dbcafetal.SaveChanges();

                return "Planta eliminada correctamente";
            }
            catch (Exception ex)
            {
                // Manejo de errores similar a ModificarEstado
                return "Error al eliminar la planta: " + ex.Message;
            }
        }

        public string ModificarEstado(int idPlanta, string nuevoEstado)
        {
            try
            {
                var planta = dbcafetal.PLANTAs.Find(idPlanta);
                if (planta == null)
                    return "Planta no encontrada";

                planta.estado = nuevoEstado;
                dbcafetal.SaveChanges();
                return "Estado actualizado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al modificar estado: " + ex.Message;
            }
        }

        public string ModificarObservaciones(int idPlanta, string nuevasObservaciones)
        {
            try
            {
                var planta = dbcafetal.PLANTAs.Find(idPlanta);
                if (planta == null)
                    return "Planta no encontrada";

                planta.observaciones = nuevasObservaciones;
                dbcafetal.SaveChanges();
                return "Observaciones actualizadas correctamente";
            }
            catch (Exception ex)
            {
                return "Error al modificar observaciones: " + ex.Message;
            }
        }

        public List<PLANTA> ConsultarPorLote(int idLote)
        {
            try
            {
                return dbcafetal.PLANTAs.Where(p => p.id_lote == idLote).ToList();
            }
            catch
            {
                return new List<PLANTA>();
            }
        }

        public List<PLANTA> ConsultarTodas()
        {
            try
            {
                return dbcafetal.PLANTAs.ToList();
            }
            catch
            {
                return new List<PLANTA>();
            }
        }

        // Métodos auxiliares
        private bool LoteExiste(int idLote)
        {
            return dbcafetal.LOTEs.Any(l => l.id_lote == idLote);
        }

        private bool PlantaExiste(int idPlanta)
        {
            return dbcafetal.PLANTAs.Any(p => p.id_planta == idPlanta);
        }
    }
}