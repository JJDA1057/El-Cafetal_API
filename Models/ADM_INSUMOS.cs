//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API_CAFETAL.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ADM_INSUMOS
    {
        public int id_aplica { get; set; }
        public Nullable<int> id_lote { get; set; }
        public Nullable<int> id_insumo { get; set; }
        public Nullable<int> cant_usada { get; set; }
        public Nullable<System.DateTime> fecha_aplic { get; set; }
        public Nullable<int> id_proveedor { get; set; }
    
        public virtual INSUMO INSUMO { get; set; }
        public virtual LOTE LOTE { get; set; }
        public virtual PROVEEDOR PROVEEDOR { get; set; }
    }
}
