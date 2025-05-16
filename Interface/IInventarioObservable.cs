using API_CAFETAL.Models;
using El_Cafetal_APP.Clases.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static API_CAFETAL.Clases.clsInsumo;

namespace API_CAFETAL.Interface
{
    public interface IInventarioObservable
    {

        void AgregarObservador(IInventarioObserver observador);
        void EliminarObservador(IInventarioObserver observador);
        void NotificarObservadores(INSUMO insumo, TipoAlerta tipoAlerta);

    }
    public interface IInventarioObserver
    {
        void Actualizar(INSUMO insumo, TipoAlerta tipoAlerta);
    }
}