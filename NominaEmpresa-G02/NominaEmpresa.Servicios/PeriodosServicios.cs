using NominaEmpresa.Dominio;
using NominaEmpresa.Persistencia;
using System.Collections.Generic;

namespace NominaEmpresa.Servicios
{
    public class PeriodosServicio
    {
        private readonly PeriodosDAO _periodosDAO;

        public PeriodosServicio()
        {
            _periodosDAO = new PeriodosDAO();
        }

        // Obtener Periodos Activos
        public List<Periodos> ObtenerPeriodosActivos()
        {
            return _periodosDAO.ListarActivos();
        }

        // Obtener Periodos Inactivos
        public List<Periodos> ObtenerPeriodosInactivos()
        {
            return _periodosDAO.ListarInactivos();
        }

        // Guardar Periodo (Insertar o Actualizar)
        public void GuardarPeriodo(Periodos periodo)
        {
            _periodosDAO.GuardarPeriodo(periodo);
        }

        // Activar Periodo
        public void ActivarPeriodo(int periodoCodigo)
        {
            _periodosDAO.ActivarPeriodo(periodoCodigo);
        }

        // Cerrar Periodo Activo
        public void CerrarPeriodoActivo()
        {
            _periodosDAO.CerrarPeriodoActivo();
        }



        // Obtener el próximo periodo cercano al activo
        public Periodos ObtenerProximoPeriodoActivo()
        {
            return _periodosDAO.ObtenerProximoPeriodoActivo();
        }

    }



}