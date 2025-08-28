using NominaEmpresa.Dominio;
using NominaEmpresa.Servicios;
using System.Collections.Generic;

namespace NominaEmpresa.Aplicacion
{
    public class ContratosLaboralesAplicacion
    {
        private readonly ContratosLaboralesServicio _contratosServicio;

        public ContratosLaboralesAplicacion()
        {
            _contratosServicio = new ContratosLaboralesServicio();
        }

        // Método para obtener contratos activos
        public List<ContratoLaborales> ObtenerContratosActivos()
        {
            return _contratosServicio.ObtenerContratosActivos();
        }

        // Método para obtener contratos inactivos
        public List<ContratoLaborales> ObtenerContratosInactivos()
        {
            return _contratosServicio.ObtenerContratosInactivos();
        }

        // Método para obtener un contrato por ID
        public ContratoLaborales ObtenerContratoPorId(int contratoCodigo)
        {
            return _contratosServicio.ObtenerContratoPorId(contratoCodigo);
        }

        // Método para crear un nuevo contrato
        public int CrearContrato(ContratoLaborales contrato)
        {
            return _contratosServicio.CrearContrato(contrato);
        }

        // Método para actualizar un contrato
        public void ActualizarContrato(ContratoLaborales contrato)
        {
            _contratosServicio.ActualizarContrato(contrato);
        }

        // Método para eliminar (inhabilitar) un contrato
        public void EliminarContrato(int contratoCodigo)
        {
            _contratosServicio.EliminarContrato(contratoCodigo);
        }

        // Método para recuperar un contrato inactivo
        public void RecuperarContrato(int contratoCodigo)
        {
            _contratosServicio.RecuperarContrato(contratoCodigo);
        }

        // Métodos para obtener datos de catálogos
        public List<TipoContratos> ObtenerTiposContrato(int? codigo = null)
        {
            return _contratosServicio.ObtenerTiposContrato(codigo);
        }

        public List<Departamentos> ObtenerDepartamentos(int? codigo = null)
        {
            return _contratosServicio.ObtenerDepartamentos(codigo);
        }

        public List<Cargos> ObtenerCargos(int? codigo = null)
        {
            return _contratosServicio.ObtenerCargos(codigo);
        }

        public List<ModalidadTrabajo> ObtenerModalidades(int? codigo = null)
        {
            return _contratosServicio.ObtenerModalidades(codigo);
        }

        public List<Trabajadores> ObtenerTrabajadores(int? codigo = null)
        {
            return _contratosServicio.ObtenerTrabajadores(codigo);
        }

        public List<Trabajadores> ObtenerTrabajadoresE(int? codigo = null)
        {
            return _contratosServicio.ObtenerTrabajadoresE(codigo);
        }
    }
}