
using API_CAFETAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace API_CAFETAL.Clases
{
    public class clsProveedor
    {
        private CAFETALDBEntities dbcafetal = new CAFETALDBEntities();
        public PROVEEDOR proveedor { get; set; }

        // 1. Registrar nuevo proveedor
        public string RegistrarProveedor(string nombre, string celFijo, string tipoMaterial)
        {
            try
            {
                if (ExisteProveedor(nombre))
                    return "El proveedor ya existe";

                var nuevoProveedor = new PROVEEDOR
                {
                    nombre = nombre,
                    cel_fijo = celFijo,
                    tipo_material = tipoMaterial
                };

                dbcafetal.PROVEEDORs.Add(nuevoProveedor);
                dbcafetal.SaveChanges();
                return "Proveedor registrado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al registrar: " + ex.Message;
            }
        }

        // 2. Consultar todos los proveedores
        public List<PROVEEDOR> ConsultarTodos()
        {
            return dbcafetal.PROVEEDORs
                .OrderBy(p => p.nombre)
                .ToList();
        }

        // 3. Consultar proveedor por ID
        public PROVEEDOR ConsultarPorId(int idProveedor)
        {
            return dbcafetal.PROVEEDORs
                .FirstOrDefault(p => p.id_proveedor == idProveedor);
        }

        // 4. Consultar proveedores por tipo de material
        public List<PROVEEDOR> ConsultarPorTipoMaterial(string tipoMaterial)
        {
            return dbcafetal.PROVEEDORs
                .Where(p => p.tipo_material.Contains(tipoMaterial))
                .OrderBy(p => p.nombre)
                .ToList();
        }

        // 5. Actualizar proveedor
        public string ActualizarProveedor(int idProveedor, string nombre, string celFijo, string tipoMaterial)
        {
            try
            {
                var proveedor = dbcafetal.PROVEEDORs.Find(idProveedor);
                if (proveedor == null)
                    return "Proveedor no encontrado";

                proveedor.nombre = nombre;
                proveedor.cel_fijo = celFijo;
                proveedor.tipo_material = tipoMaterial;

                dbcafetal.SaveChanges();
                return "Proveedor actualizado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al actualizar: " + ex.Message;
            }
        }

        // 6. Eliminar proveedor (con validación de relaciones)
        public string EliminarProveedor(int idProveedor)
        {
            try
            {
                var proveedor = dbcafetal.PROVEEDORs
                    .Include("ADM_INSUMOS") // Carga relacionadas para validar
                    .FirstOrDefault(p => p.id_proveedor == idProveedor);

                if (proveedor == null)
                    return "Proveedor no encontrado";

                if (proveedor.ADM_INSUMOS.Any())
                    return "No se puede eliminar, tiene insumos asociados";

                dbcafetal.PROVEEDORs.Remove(proveedor);
                dbcafetal.SaveChanges();
                return "Proveedor eliminado correctamente";
            }
            catch (Exception ex)
            {
                return "Error al eliminar: " + ex.Message;
            }
        }

        // 7. Validar existencia de proveedor
        public bool ExisteProveedor(string nombre)
        {
            return dbcafetal.PROVEEDORs
                .Any(p => p.nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        }

    }
}