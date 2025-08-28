using NominaEmpresa.Dominio;
using NominaEmpresa.Persistencia;
using System;
using System.Collections.Generic;

namespace NominaEmpresa.Servicios
{
    public class ContratosLaboralesServicio
    {
        private readonly ContratosLaboralesDAO _contratosDAO;
        private const decimal SUELDO_MINIMO_PERU = 1025.00M;

        public ContratosLaboralesServicio()
        {
            _contratosDAO = new ContratosLaboralesDAO();
        }

        public List<ContratoLaborales> ObtenerContratosActivos()
        {
            return _contratosDAO.ListarActivos();
        }

        public List<ContratoLaborales> ObtenerContratosInactivos()
        {
            return _contratosDAO.ListarInactivos();
        }

        public ContratoLaborales ObtenerContratoPorId(int contratoCodigo)
        {
            return _contratosDAO.ObtenerPorId(contratoCodigo);
        }

        private void ValidarDatosContrato(ContratoLaborales contrato)
        {
            if (contrato.TrabajadoresCodigo <= 0)
                throw new Exception("Debe seleccionar un trabajador válido.");

            if (contrato.CargosCodigo <= 0)
                throw new Exception("Debe seleccionar un cargo válido.");

            if (contrato.TipoContratosCodigo <= 0)
                throw new Exception("Debe seleccionar un tipo de contrato válido.");

            if (contrato.ModalidadCodigo <= 0)
                throw new Exception("Debe seleccionar una modalidad de trabajo válida.");

            if (contrato.SueldoBase <= 0)
                throw new Exception("El sueldo base debe ser mayor a 0.");

            if (contrato.SueldoBase < SUELDO_MINIMO_PERU)
                throw new Exception($"El sueldo base no puede ser menor al sueldo mínimo vital de Perú (S/. {SUELDO_MINIMO_PERU}).");
        }

        private void ValidarFechasContrato(ContratoLaborales contrato)
        {
            if (contrato.FechaInicio < DateTime.Now.Date)
                throw new Exception("La fecha de inicio no puede ser anterior a la fecha actual.");

            if (contrato.FechaFin.HasValue && contrato.FechaFin.Value <= contrato.FechaInicio)
                throw new Exception("La fecha de fin debe ser posterior a la fecha de inicio.");
        }


        private void ValidarHorarioLaboral(ContratoLaborales contrato)
        {
            if (string.IsNullOrEmpty(contrato.DiaInicioJornada) || string.IsNullOrEmpty(contrato.DiaFinJornada))
                throw new Exception("Debe especificar los días de la jornada laboral.");

            if (contrato.HoraInicioJornada >= contrato.HoraFinJornada)
                throw new Exception("La hora de inicio debe ser anterior a la hora de fin de jornada.");
        }

        public int CrearContrato(ContratoLaborales contrato)
        {
            ValidarDatosContrato(contrato);
            ValidarFechasContrato(contrato);
            ValidarHorarioLaboral(contrato);

            contrato.EstadoContrato = 'A'; 

            return _contratosDAO.CrearContrato(contrato);
        }

        public void ActualizarContrato(ContratoLaborales contrato)
        {
 
            ValidarDatosContrato(contrato);
            ValidarHorarioLaboral(contrato);


            _contratosDAO.ActualizarContrato(contrato);
        }

        public void EliminarContrato(int contratoCodigo)
        {
            var contrato = _contratosDAO.ObtenerPorId(contratoCodigo);
            if (contrato == null)
                throw new Exception("No se encontró el contrato especificado.");

            _contratosDAO.EliminarContrato(contratoCodigo);
        }

        public void RecuperarContrato(int contratoCodigo)
        {
            var contrato = _contratosDAO.ObtenerPorId(contratoCodigo);
            if (contrato == null)
                throw new Exception("No se encontró el contrato especificado.");


            _contratosDAO.RecuperarContrato(contratoCodigo);
        }

        public List<TipoContratos> ObtenerTiposContrato(int? codigo = null)
        {
            return _contratosDAO.ObtenerTiposContrato(codigo);
        }

        public List<Departamentos> ObtenerDepartamentos(int? codigo = null)
        {
            return _contratosDAO.ObtenerDepartamentos(codigo);
        }

        public List<Cargos> ObtenerCargos(int? codigo = null)
        {
            return _contratosDAO.ObtenerCargos(codigo);
        }

        public List<ModalidadTrabajo> ObtenerModalidades(int? codigo = null)
        {
            return _contratosDAO.ObtenerModalidades(codigo);
        }

        public List<Trabajadores> ObtenerTrabajadores(int? codigo = null)
        {
            return _contratosDAO.ObtenerTrabajadores(codigo);
        }

        public List<Trabajadores> ObtenerTrabajadoresE(int? codigo = null)
        {
            return _contratosDAO.ObtenerTrabajadores(codigo);
        }
    }

}