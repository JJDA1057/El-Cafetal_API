
using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;

namespace API_CAFETAL.Clases
{
    public class clsAdmInsumos
    {
        private CAFETALDBEntities dbcafetal = new CAFETALDBEntities();
        public ADM_INSUMOS admInsumo { get; set; }

        // 1. Registrar aplicación de insumo
        public string RegistrarAplicacion(int idAplica, int idLote, int idInsumo, decimal cantUsada, DateTime fechaAplic, int idProveedor)
        {
            using (var transaction = dbcafetal.Database.BeginTransaction())
            {
                try
                {
                    // Validaciones (las que ya tenías)
                    if (!LoteExiste(idLote))
                        return "El lote no existe";

                    if (!InsumoExiste(idInsumo))
                        return "El insumo no existe";

                    if (!ProveedorExiste(idProveedor))
                        return "El proveedor no existe";

                    var insumo = dbcafetal.INSUMOes.Find(idInsumo);
                    if (insumo.cantidad < cantUsada)
                        return $"Cantidad insuficiente. Stock actual: {insumo.cantidad}";

                    // Registrar la aplicación
                    var nuevaAplicacion = new ADM_INSUMOS
                    {
                        id_aplica = idAplica,
                        id_lote = idLote,
                        id_insumo = idInsumo,
                        cant_usada = Convert.ToInt32(cantUsada),
                        fecha_aplic = fechaAplic,
                        id_proveedor = idProveedor
                    };

                    dbcafetal.ADM_INSUMOS.Add(nuevaAplicacion);

                    // Actualizar stock
                    insumo.cantidad -= Convert.ToInt32(cantUsada);

                    dbcafetal.SaveChanges();
                    transaction.Commit();

                    return "Aplicación registrada correctamente";
                }
                catch (DbEntityValidationException ex)
                {
                    transaction.Rollback();
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                    return $"Error de validación: {string.Join("; ", errorMessages)}";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // Capturar el inner exception para más detalles
                    return $"Error al registrar: {ex.InnerException?.Message ?? ex.Message}";
                }
            }
        }

        // 2. Consultar todas las aplicaciones (devuelve entidades completas)
        public List<ADM_INSUMOS> ConsultarTodas()
        {
            return dbcafetal.ADM_INSUMOS
                .Include("INSUMO")
                .Include("LOTE")
                .Include("PROVEEDOR")
                .OrderByDescending(a => a.fecha_aplic)
                .ToList();
        }

        // 3. Consultar aplicaciones por lote
        public List<ADM_INSUMOS> ConsultarPorLote(int idLote)
        {
            return dbcafetal.ADM_INSUMOS
                .Include("INSUMO")
                .Where(a => a.id_lote == idLote)
                .OrderBy(a => a.fecha_aplic)
                .ToList();
        }

        // 4. Consultar aplicaciones por insumo
        public List<ADM_INSUMOS> ConsultarPorInsumo(int idInsumo)
        {
            return dbcafetal.ADM_INSUMOS
                .Include("LOTE")
                .Where(a => a.id_insumo == idInsumo)
                .OrderByDescending(a => a.fecha_aplic)
                .ToList();
        }

        // 5. Consultar aplicaciones por rango de fechas
        public List<ADM_INSUMOS> ConsultarPorFecha(DateTime desde, DateTime hasta)
        {
            return dbcafetal.ADM_INSUMOS
                .Include("INSUMO")
                .Include("LOTE")
                .Where(a => a.fecha_aplic >= desde && a.fecha_aplic <= hasta)
                .ToList();
        }

        // 6. Revertir aplicación (eliminar registro y devolver stock)
        public string RevertirAplicacion(int idAplica)
        {
            try
            {
                var aplicacion = dbcafetal.ADM_INSUMOS.Find(idAplica);
                if (aplicacion == null)
                    return "Registro no encontrado";

                // Devolver cantidad al stock
                var insumo = dbcafetal.INSUMOes.Find(aplicacion.id_insumo);
                insumo.cantidad += aplicacion.cant_usada;

                dbcafetal.ADM_INSUMOS.Remove(aplicacion);
                dbcafetal.SaveChanges();
                return "Aplicación revertida correctamente";
            }
            catch (Exception ex)
            {
                return "Error al revertir: " + ex.Message;
            }
        }

        // 7. Obtener insumos más utilizados (versión sin DTO)
        public dynamic ObtenerInsumosMasUsados(int top = 5)
        {
            return dbcafetal.ADM_INSUMOS
                .GroupBy(a => new { a.INSUMO.id_insumo, a.INSUMO.nombre })
                .Select(g => new
                {
                    InsumoId = g.Key.id_insumo,
                    Nombre = g.Key.nombre,
                    TotalUsado = g.Sum(a => a.cant_usada),
                    VecesUtilizado = g.Count()
                })
                .OrderByDescending(x => x.TotalUsado)
                .Take(top)
                .ToList();
        }

        // 8. Obtener consumo mensual por tipo de insumo (versión sin DTO)
        public dynamic ObtenerConsumoMensual()
        {
            var fechaActual = DateTime.Now;
          

            return dbcafetal.ADM_INSUMOS
                .GroupBy(a => new {
                    Year = a.fecha_aplic.HasValue ? a.fecha_aplic.Value.Year : fechaActual.Year,
                    Month = a.fecha_aplic.HasValue ? a.fecha_aplic.Value.Month : fechaActual.Month,
                    Tipo = a.INSUMO.tipo
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    g.Key.Tipo,
                    TotalConsumido = g.Sum(a => a.cant_usada)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();
        }

        // Método en clsAdm_insumos para consultar consumo de fertilizantes por lote
        public List<dynamic> ConsultarFertilizantesPorLote(int idLote)
        {
            return dbcafetal.ADM_INSUMOS
            .Include("LOTE")
            .Include("INSUMO")
            .Where(a => a.id_lote == idLote && a.INSUMO.tipo == "FERTILIZANTE")
            .OrderByDescending(a => a.fecha_aplic)
            .Select(a => new
            {
                LoteId = a.id_lote,
                Variedad = a.LOTE.variedad,
                Area = a.LOTE.area,
                Fertilizante = a.INSUMO.nombre,
                Cantidad = a.cant_usada,
                Unidad = a.INSUMO.nombre,
                Fecha = a.fecha_aplic
            })
            .ToList<dynamic>();
        }

        // Métodos de validación
        private bool LoteExiste(int idLote) => dbcafetal.LOTEs.Any(l => l.id_lote == idLote);
        private bool InsumoExiste(int idInsumo) => dbcafetal.INSUMOes.Any(i => i.id_insumo == idInsumo);
        private bool ProveedorExiste(int idProveedor) => dbcafetal.PROVEEDORs.Any(p => p.id_proveedor == idProveedor);
    }
}