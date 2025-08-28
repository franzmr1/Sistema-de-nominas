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

        public List<ContratoLaborales> ObtenerContratosActivos()
        {
            return _contratosServicio.ObtenerContratosActivos();
        }

        public List<ContratoLaborales> ObtenerContratosInactivos()
        {
            return _contratosServicio.ObtenerContratosInactivos();
        }

        public ContratoLaborales ObtenerContratoPorId(int contratoCodigo)
        {
            return _contratosServicio.ObtenerContratoPorId(contratoCodigo);
        }

        public int CrearContrato(ContratoLaborales contrato)
        {
            return _contratosServicio.CrearContrato(contrato);
        }

        public void ActualizarContrato(ContratoLaborales contrato)
        {
            _contratosServicio.ActualizarContrato(contrato);
        }

        public void EliminarContrato(int contratoCodigo)
        {
            _contratosServicio.EliminarContrato(contratoCodigo);
        }

        public void RecuperarContrato(int contratoCodigo)
        {
            _contratosServicio.RecuperarContrato(contratoCodigo);
        }

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