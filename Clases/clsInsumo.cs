using API_CAFETAL.Models;
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


    }
}