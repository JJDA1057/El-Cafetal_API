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

            // 1. Registrar nuevo lote
            public string RegistrarLote(int idLote, int idUsuario, string variedad, DateTime inicioSiembra, string estado,
                                      DateTime? fechaFinSiembra, decimal area, int cantidadPlantas,
                                      string observaciones, int idCultivo)
            {
                try
                {
                    // Validaciones básicas
                    if (string.IsNullOrWhiteSpace(variedad))
                        return "La variedad es requerida";

                    if (area <= 0)
                        return "El área debe ser mayor a cero";

                    if (!UsuarioExiste(idUsuario))
                        return "El usuario no existe";

                    if (!CultivoExiste(idCultivo))
                        return "El cultivo no existe";

                    var nuevoLote = new LOTE
                    {
                        id_lote=idLote,
                        id_usuario = idUsuario,
                        variedad = variedad,
                        inicio_siembra = inicioSiembra,
                        estado = estado,
                        fecha_fin_siembra = fechaFinSiembra,
                        area = area,
                        cantidad_plantas = cantidadPlantas,
                        observaciones = observaciones,
                        id_cultivo = idCultivo
                    };

                    dbcafetal.LOTEs.Add(nuevoLote);
                    dbcafetal.SaveChanges();
                    return "Lote registrado correctamente";
                }
                catch (Exception ex)
                {
                    return "Error al registrar: " + ex.Message;
                }
            }

            // 2. Actualizar lote
            public string ActualizarLote(int idLote, int idUsuario, string variedad, DateTime inicioSiembra,
                                        string estado, DateTime? fechaFinSiembra, decimal area,
                                        int cantidadPlantas, string observaciones, int idCultivo)
            {
                try
                {
                    var lote = dbcafetal.LOTEs.Find(idLote);
                    if (lote == null)
                        return "Lote no encontrado";

                    // Validaciones
                    if (!UsuarioExiste(idUsuario))
                        return "Usuario no existe";

                    if (!CultivoExiste(idCultivo))
                        return "Cultivo no existe";

                    lote.id_usuario = idUsuario;
                    lote.variedad = variedad;
                    lote.inicio_siembra = inicioSiembra;
                    lote.estado = estado;
                    lote.fecha_fin_siembra = fechaFinSiembra;
                    lote.area = area;
                    lote.cantidad_plantas = cantidadPlantas;
                    lote.observaciones = observaciones;
                    lote.id_cultivo = idCultivo;

                    dbcafetal.SaveChanges();
                    return "Lote actualizado correctamente";
                }
                catch (Exception ex)
                {
                    return "Error al actualizar: " + ex.Message;
                }
            }

            // 3. Consultar todos los lotes (sin ViewModel)
            public List<LOTE> ConsultarTodos()
            {
                return dbcafetal.LOTEs
                    .Include("USUARIO")
                    .Include("CULTIVO")
                    .OrderBy(l => l.estado)
                    .ThenBy(l => l.inicio_siembra)
                    .ToList();
            }

            // 4. Consultar lote por ID
            public LOTE ConsultarPorId(int idLote)
            {
                return dbcafetal.LOTEs
                    .Include("USUARIO")
                    .Include("CULTIVO")
                    .FirstOrDefault(l => l.id_lote == idLote);
            }

            // 5. Consultar lotes por estado
            public List<LOTE> ConsultarPorEstado(string estado)
            {
                return dbcafetal.LOTEs
                    .Where(l => l.estado.Contains(estado))
                    .OrderBy(l => l.inicio_siembra)
                    .ToList();
            }

            // 6. Consultar lotes por cultivo
            public List<LOTE> ConsultarPorCultivo(int idCultivo)
            {
                return dbcafetal.LOTEs
                    .Include("USUARIO")
                    .Where(l => l.id_cultivo == idCultivo)
                    .OrderByDescending(l => l.inicio_siembra)
                    .ToList();
            }

            // 7. Cambiar estado de lote
            public string CambiarEstado(int idLote, string nuevoEstado, DateTime? fechaFin = null)
            {
                try
                {
                    var lote = dbcafetal.LOTEs.Find(idLote);
                    if (lote == null)
                        return "Lote no encontrado";

                    lote.estado = nuevoEstado;
                    if (fechaFin.HasValue)
                        lote.fecha_fin_siembra = fechaFin;

                    dbcafetal.SaveChanges();
                    return "Estado actualizado correctamente";
                }
                catch (Exception ex)
                {
                    return "Error al cambiar estado: " + ex.Message;
                }
            }

            // 8. Obtener resumen por variedad (sin ViewModel)
            public dynamic ObtenerResumenPorVariedad()
            {
                return dbcafetal.LOTEs
                    .GroupBy(l => l.variedad)
                    .Select(g => new
                    {
                        Variedad = g.Key,
                        TotalLotes = g.Count(),
                        AreaTotal = g.Sum(l => l.area),
                        PlantasTotal = g.Sum(l => l.cantidad_plantas)
                    })
                    .OrderByDescending(x => x.AreaTotal)
                    .ToList();
            }

            // 9. Obtener lotes próximos a cosechar
            public List<LOTE> ObtenerLotesProximosACosechar(int diasAntelacion = 30)
            {
                var fechaReferencia = DateTime.Now.AddDays(diasAntelacion);
                return dbcafetal.LOTEs
                    .Include("CULTIVO")
                    .Where(l => l.estado == "En crecimiento" &&
                               l.fecha_fin_siembra.HasValue &&
                               l.fecha_fin_siembra <= fechaReferencia)
                    .OrderBy(l => l.fecha_fin_siembra)
                    .ToList();
            }

            // 10. Obtener productividad por lote
            public dynamic ObtenerProductividad()
            {
                return dbcafetal.LOTEs
                    .Include("CULTIVO")
                    .AsEnumerable() // Para cálculos en memoria
                    .GroupBy(l => l.CULTIVO.nombre)
                    .Select(g => new
                    {
                        Cultivo = g.Key,
                        RendimientoPromedio = g.Average(l => l.area > 0 ? l.cantidad_plantas / l.area : 0),
                        LotesActivos = g.Count(l => l.estado == "Activo")
                    })
                    .ToList();
            }

           

            // 12. Métodos de validación
            private bool UsuarioExiste(int idUsuario) => dbcafetal.USUARIOs.Any(u => u.id_usuario == idUsuario);
            private bool CultivoExiste(int idCultivo) => dbcafetal.CULTIVOes.Any(c => c.id_cultivo == idCultivo);
        }
 }