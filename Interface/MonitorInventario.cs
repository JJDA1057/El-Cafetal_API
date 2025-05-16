using API_CAFETAL.Clases;
using API_CAFETAL.Controllers;
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
    public class MonitorInventario : IInventarioObservable
    {
        private List<IInventarioObserver> _observadores = new List<IInventarioObserver>();
        private clsInsumo _servicioInsumo;

        public MonitorInventario(clsInsumo servicioInsumo)
        {
            _servicioInsumo = servicioInsumo;
        }

        public void AgregarObservador(IInventarioObserver observador)
        {
            if (!_observadores.Contains(observador))
                _observadores.Add(observador);
        }

        public void EliminarObservador(IInventarioObserver observador)
        {
            if (_observadores.Contains(observador))
                _observadores.Remove(observador);
        }

        public void NotificarObservadores(INSUMO insumo, TipoAlerta tipoAlerta)
        {
            foreach (var observador in _observadores)
            {
                observador.Actualizar(insumo, tipoAlerta);
            }
        }

        // Verificar todos los insumos y notificar sobre niveles bajos
        public void VerificarInventario()
        {
            var insumosNivelesBajos = _servicioInsumo.ConsultarNivelesBajos();
            var insumosNivelesCriticos = _servicioInsumo.ConsultarNivelesCriticos();

            var idsCriticos = new HashSet<int>(insumosNivelesCriticos.Select(i => i.id_insumo));

            //Notificar primero insumos criticos

            foreach (var insumo in insumosNivelesCriticos)
            {
                NotificarObservadores(insumo, TipoAlerta.Critica);
            }

            //Notificar  insumos bajos
            foreach (var insumo in insumosNivelesBajos)
            {
                if (!idsCriticos.Contains(insumo.id_insumo))
                {
                    NotificarObservadores(insumo, TipoAlerta.Baja);
                }
            }
        }

        // Verificar un insumo específico
        public void VerificarInsumo(int idInsumo)
        {
            INSUMO insumo = _servicioInsumo.ConsultarPorId(idInsumo);
            if (insumo == null)
                return;

            TipoAlerta tipoAlerta = _servicioInsumo.VerificarNivelPorId(idInsumo);

            if (tipoAlerta != TipoAlerta.Normal)
            {
                NotificarObservadores(insumo, tipoAlerta);
            }
        }

        // Este método se puede llamar después de cada actualización de cantidad
        public void VerificarDespuesDeActualizar(int idInsumo)
        {
            VerificarInsumo(idInsumo);
        }
    }
}