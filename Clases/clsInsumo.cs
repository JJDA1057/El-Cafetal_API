using API_CAFETAL.Models;
using El_Cafetal_APP.Clases.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace API_CAFETAL.Clases
{
    public class clsInsumo
    {

        private CAFETALDBEntities dbcafetal = new CAFETALDBEntities();
        public INSUMO insumo { get; set; }


        public string RegistrarInsumo(string nombre, string tipo, int idProveedor, int cantidad, DateTime fEntrega)
        {
            try
            {
                if (ExisteInsumo(nombre))
                    return "El insumo ya existe";

                if (!ProveedorExiste(idProveedor))
                    return "El proveedor no existe";

                var nuevoInsumo = new INSUMO
                {
                    nombre = nombre,
                    tipo = tipo,
                    id_proveedor = idProveedor,
                    cantidad = cantidad,
                    f_entrega = fEntrega
                };

                dbcafetal.INSUMOes.Add(nuevoInsumo);
                dbcafetal.SaveChanges();
                return "Insumo registrado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al registrar: " + ex.Message;
            }
        }

        // 2. Consultar todos los insumos (ahora devuelve List<INSUMO> directamente)
        public List<INSUMO> ConsultarTodos()
        {
            return dbcafetal.INSUMOes
                .Include("PROVEEDOR") // <- Carga relacionada
                .OrderBy(i => i.nombre)
                .ToList();
        }

        // 3. Consultar insumo por ID (igual que antes)
        public INSUMO ConsultarPorId(int idInsumo)
        {
            return dbcafetal.INSUMOes
                .Include("PROVEEDOR")
                .FirstOrDefault(i => i.id_insumo == idInsumo);
        }

        // 4. Consultar insumos por tipo (igual que antes)
        public List<INSUMO> ConsultarPorTipo(string tipo)
        {
            return dbcafetal.INSUMOes
                .Where(i => i.tipo.Contains(tipo))
                .OrderBy(i => i.nombre)
                .ToList();
        }

        // 5. Consultar insumos por proveedor (igual que antes)
        public List<INSUMO> ConsultarPorProveedor(int idProveedor)
        {
            return dbcafetal.INSUMOes
                .Where(i => i.id_proveedor == idProveedor)
                .OrderBy(i => i.nombre)
                .ToList();
        }

        // 6. Actualizar insumo (igual que antes)
        public string ActualizarInsumo(int idInsumo, string nombre, string tipo, int idProveedor, int cantidad, DateTime fEntrega)
        {
            try
            {
                var insumo = dbcafetal.INSUMOes.Find(idInsumo);
                if (insumo == null)
                    return "Insumo no encontrado";

                if (!ProveedorExiste(idProveedor))
                    return "El proveedor no existe";

                insumo.nombre = nombre;
                insumo.tipo = tipo;
                insumo.id_proveedor = idProveedor;
                insumo.cantidad = cantidad;
                insumo.f_entrega = fEntrega;

                dbcafetal.SaveChanges();
                return "Insumo actualizado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al actualizar: " + ex.Message;
            }
        }

        // 7. Eliminar insumo (igual que antes)
        public string EliminarInsumo(int idInsumo)
        {
            try
            {
                var insumo = dbcafetal.INSUMOes
                    .Include("ADM_INSUMOS")
                    .FirstOrDefault(i => i.id_insumo == idInsumo);

                if (insumo == null)
                    return "Insumo no encontrado";

                if (insumo.ADM_INSUMOS.Any())
                    return "No se puede eliminar, tiene registros en ADM_INSUMOS asociados";

                dbcafetal.INSUMOes.Remove(insumo);
                dbcafetal.SaveChanges();
                return "Insumo eliminado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al eliminar: " + ex.Message;
            }
        }

        // 8. Validar existencia de insumo (igual que antes)
        public bool ExisteInsumo(string nombre)
        {
            return dbcafetal.INSUMOes
                .Any(i => i.nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        }

        // 9. Validar existencia de proveedor (igual que antes)
        private bool ProveedorExiste(int idProveedor)
        {
            return dbcafetal.PROVEEDORs.Any(p => p.id_proveedor == idProveedor);
        }

        // 10. Actualizar cantidad de insumo (igual que antes)
        public string ActualizarCantidad(int idInsumo, int cantidadModificada, bool esSuma)
        {
            try
            {
                var insumo = dbcafetal.INSUMOes.Find(idInsumo);
                if (insumo == null)
                    return "Insumo no encontrado";

                insumo.cantidad = esSuma ?
                    insumo.cantidad + cantidadModificada :
                    insumo.cantidad - cantidadModificada;

                if (insumo.cantidad < 0)
                    return "No hay suficiente cantidad disponible";

                dbcafetal.SaveChanges();
                return "Cantidad actualizada correctamente";
            }
            catch (Exception ex)
            {
                return "Error al actualizar cantidad: " + ex.Message;
            }

        }

        //----------------------------------------------------------------Observer


        // Método para consultar insumos con niveles bajos
        public List<INSUMO> ConsultarNivelesBajos()
        {
            var resultado = new List<INSUMO>();

            foreach (var insumo in dbcafetal.INSUMOes.Include("PROVEEDOR").ToList())
            {
                string tipoClave = DeterminarTipoBase(insumo.tipo);

                if (_umbralBajo.ContainsKey(tipoClave) && _umbralCritico.ContainsKey(tipoClave))
                {
                    int umbralBajo = _umbralBajo[tipoClave];
                    int umbralCritico = _umbralCritico[tipoClave];

                    // Solo considerar como bajo si no es crítico
                    if (insumo.cantidad <= umbralBajo && insumo.cantidad > umbralCritico)
                    {
                        resultado.Add(insumo);
                    }
                }
            }

            return resultado;
        }


        public List<INSUMO> ConsultarNivelesCriticos()
        {
            var resultado = new List<INSUMO>();

            foreach (var insumo in dbcafetal.INSUMOes.Include("PROVEEDOR").ToList())
            {
                string tipoClave = DeterminarTipoBase(insumo.tipo);

                if (_umbralCritico.ContainsKey(tipoClave))
                {
                    int umbral = _umbralCritico[tipoClave];

                    if (insumo.cantidad <= umbral)
                    {
                        resultado.Add(insumo);
                    }
                }
            }

            return resultado;
        }


        // Método para configurar umbrales personalizados
        //public static void ConfigurarUmbral(string tipo, int umbralBajo, int umbralCritico)
        //{
        //    _umbralBajo[tipo] = umbralBajo;
        //    _umbralCritico[tipo] = umbralCritico;
        //}

        // Método auxiliar para determinar el tipo base de un insumo
        private string DeterminarTipoBase(string tipoCompleto)
        {
            tipoCompleto = tipoCompleto.ToLowerInvariant().Trim();

            string[] tiposBase = { "semilla", "nutriente", "abono", "fertilizante", "vitamina", "mineral" };

            foreach (var tipo in tiposBase)
            {
                if (tipoCompleto.Contains(tipo))
                {
                    return tipo;
                }
            }

            return tipoCompleto;
        }


        // Método para verificar nivel por ID
        public TipoAlerta VerificarNivelPorId(int idInsumo)
        {
            var insumo = ConsultarPorId(idInsumo);
            if (insumo == null)
                return TipoAlerta.Normal;

            string tipoBase = DeterminarTipoBase(insumo.tipo);

            // Verificar nivel crítico primero
            if (_umbralCritico.ContainsKey(tipoBase) && insumo.cantidad <= _umbralCritico[tipoBase])
                return TipoAlerta.Critica;

            // Luego verificar nivel bajo
            if (_umbralBajo.ContainsKey(tipoBase) && insumo.cantidad <= _umbralBajo[tipoBase])
                return TipoAlerta.Baja;

            return TipoAlerta.Normal;
        }


        private static Dictionary<string, int> _umbralBajo = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        private static Dictionary<string, int> _umbralCritico = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        static clsInsumo()
        {
            // Configuración inicial para semillas
            _umbralBajo["semilla"] = 30;
            _umbralCritico["semilla"] = 15;

            // Puedes agregar más tipos según sea necesario
            _umbralBajo["nutriente"] = 20;
            _umbralCritico["nutriente"] = 10;

            _umbralBajo["abono"] = 20;
            _umbralCritico["abono"] = 10;

            _umbralBajo["fertilizante"] = 20;
            _umbralCritico["fertilizante"] = 10;
        }

        

    }
}