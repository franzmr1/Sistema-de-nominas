using NominaEmpresa.Dominio;
using NominaEmpresa.Persistencia;
using System;
using System.Collections.Generic;

namespace NominaEmpresa.Servicios
{
    public class TrabajadoresServicio
    {
        private readonly TrabajadoresDAO _trabajadoresDAO;

        public TrabajadoresServicio()
        {
            _trabajadoresDAO = new TrabajadoresDAO();
        }

        // Listar Trabajadores Activos
        public List<Trabajadores> ObtenerTrabajadoresActivos()
        {
            return _trabajadoresDAO.ListarActivos();
        }

        // Listar Trabajadores Inactivos
        public List<Trabajadores> ObtenerTrabajadoresInactivos()
        {
            return _trabajadoresDAO.ListarInactivos();
        }

        // Obtener Trabajador por ID
        public Trabajadores ObtenerTrabajadorPorId(int trabajadorCodigo)
        {
            return _trabajadoresDAO.ObtenerTrabajadorPorId(trabajadorCodigo);
        }

        // Validar número de identificación único
        private void ValidarNumeroDeIdentificacionUnico(string documentoIdentidad)
        {
            var trabajadorExistente = _trabajadoresDAO.ObtenerTrabajadorPorDocumento(documentoIdentidad);
            if (trabajadorExistente != null)
            {
                throw new Exception("El número de identificación ya está registrado para otro trabajador.");
            }
        }

        // Validar datos personales completos
        private void ValidarDatosPersonalesCompletos(Trabajadores trabajador)
        {
            if (string.IsNullOrEmpty(trabajador.TrabajadoresNombre))
                throw new Exception("El nombre del trabajador es obligatorio.");

            if (string.IsNullOrEmpty(trabajador.TrabajadoresApellido))
                throw new Exception("El apellido del trabajador es obligatorio.");

            if (string.IsNullOrEmpty(trabajador.TrabajadoresDocumentoIdentidad))
                throw new Exception("El documento de identidad del trabajador es obligatorio.");

            if (string.IsNullOrEmpty(trabajador.TrabajadoresTelefono))
                throw new Exception("El teléfono del trabajador es obligatorio.");

            if (string.IsNullOrEmpty(trabajador.TrabajadoresEmail))
                throw new Exception("El email del trabajador es obligatorio.");

            if (string.IsNullOrEmpty(trabajador.TrabajadoresDireccion))
                throw new Exception("La dirección del trabajador es obligatoria.");

            if (!trabajador.TrabajadoresFechaNacimiento.HasValue)
                throw new Exception("La fecha de nacimiento del trabajador es obligatoria.");
        }

        // Validar fecha de nacimiento válida
        private void ValidarFechaNacimiento(DateTime fechaNacimiento)
        {
            // Validar que no sea una fecha futura
            if (fechaNacimiento > DateTime.Now)
            {
                throw new Exception("La fecha de nacimiento no puede ser una fecha futura o irreal.");
            }

            // Validar que no sea anterior a 1950
            if (fechaNacimiento < new DateTime(1950, 1, 1))
            {
                throw new Exception("La fecha de nacimiento no puede ser anterior al año 1950.");
            }
        }

        // Validar edad mínima
        private void ValidarEdadMinima(DateTime fechaNacimiento)
        {
            int edad = DateTime.Now.Year - fechaNacimiento.Year;
            if (DateTime.Now.AddYears(-edad) < fechaNacimiento) edad--;

            if (edad < 18)
            {
                throw new Exception("El trabajador debe ser mayor de 18 años.");
            }
        }

        // Insertar nuevo Trabajador
        public void AgregarTrabajador(Trabajadores trabajador)
        {
            try
            {
                // Validaciones de negocio
                ValidarDatosPersonalesCompletos(trabajador);
                ValidarNumeroDeIdentificacionUnico(trabajador.TrabajadoresDocumentoIdentidad);

                // Primero validamos que la fecha sea válida
                ValidarFechaNacimiento(trabajador.TrabajadoresFechaNacimiento.Value);
                // Luego validamos la edad mínima
                ValidarEdadMinima(trabajador.TrabajadoresFechaNacimiento.Value);

                // Si todas las validaciones pasan, se puede insertar
                _trabajadoresDAO.InsertarTrabajador(trabajador);
            }
            catch (Exception ex)
            {
                // Aquí puedes agregar logging si lo necesitas
                throw new Exception($"Error al agregar trabajador: {ex.Message}");
            }
        }

        // Actualizar Trabajador
        public void ActualizarTrabajador(Trabajadores trabajador)
        {
            try
            {
                // Validaciones de negocio
                ValidarDatosPersonalesCompletos(trabajador);

                var trabajadorExistente = _trabajadoresDAO.ObtenerTrabajadorPorId(trabajador.TrabajadoresCodigo);
                if (trabajadorExistente != null &&
                    trabajadorExistente.TrabajadoresDocumentoIdentidad != trabajador.TrabajadoresDocumentoIdentidad)
                {
                    ValidarNumeroDeIdentificacionUnico(trabajador.TrabajadoresDocumentoIdentidad);
                }

                // Primero validamos que la fecha sea válida
                ValidarFechaNacimiento(trabajador.TrabajadoresFechaNacimiento.Value);
                // Luego validamos la edad mínima
                ValidarEdadMinima(trabajador.TrabajadoresFechaNacimiento.Value);

                // Si todas las validaciones pasan, se puede actualizar
                _trabajadoresDAO.ActualizarTrabajador(trabajador);
            }
            catch (Exception ex)
            {
                // Aquí puedes agregar logging si lo necesitas
                throw new Exception($"Error al actualizar trabajador: {ex.Message}");
            }
        }

        // Eliminar Trabajador
        public void EliminarTrabajador(int trabajadorCodigo)
        {
            try
            {
                var trabajador = _trabajadoresDAO.ObtenerTrabajadorPorId(trabajadorCodigo);
                if (trabajador == null)
                {
                    throw new Exception("No se encontró el trabajador especificado.");
                }

                _trabajadoresDAO.EliminarTrabajador(trabajadorCodigo);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar trabajador: {ex.Message}");
            }
        }

        // Recuperar Trabajador (Cambio de estado a Activo)
        public void RecuperarTrabajador(int trabajadorCodigo)
        {
            try
            {
                var trabajador = _trabajadoresDAO.ObtenerTrabajadorPorId(trabajadorCodigo);
                if (trabajador == null)
                {
                    throw new Exception("No se encontró el trabajador especificado.");
                }

                _trabajadoresDAO.RecuperarTrabajador(trabajadorCodigo);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al recuperar trabajador: {ex.Message}");
            }
        }
    }
}