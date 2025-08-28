using NominaEmpresa.Dominio;
using NominaEmpresa.Servicios;
using System.Collections.Generic;

namespace NominaEmpresa.Aplicacion
{
    public class PeriodosAplicacion
    {
        private readonly PeriodosServicio _periodosServicio;

        public PeriodosAplicacion()
        {
            _periodosServicio = new PeriodosServicio();
        }

        // Método para obtener periodos activos
        public List<Periodos> ObtenerPeriodosActivos()
        {
            return _periodosServicio.ObtenerPeriodosActivos();
        }

        // Método para obtener periodos inactivos
        public List<Periodos> ObtenerPeriodosInactivos()
        {
            return _periodosServicio.ObtenerPeriodosInactivos();
        }

        // Método para guardar (insertar o actualizar) un periodo
        public void GuardarPeriodo(Periodos periodo)
        {
            _periodosServicio.GuardarPeriodo(periodo);
        }

        // Método para activar un periodo
        public void ActivarPeriodo(int periodoCodigo)
        {
            _periodosServicio.ActivarPeriodo(periodoCodigo);
        }

        // Método para cerrar el periodo activo
        public void CerrarPeriodoActivo()
        {
            _periodosServicio.CerrarPeriodoActivo();
        }

        // Método para obtener el próximo periodo cercano al activo
        public Periodos ObtenerProximoPeriodoActivo()
        {
            return _periodosServicio.ObtenerProximoPeriodoActivo();
        }
    }
}
