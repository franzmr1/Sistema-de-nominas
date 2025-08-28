using NominaEmpresa.Dominio;
using NominaEmpresa.Persistencia;
using System;
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

        public List<Periodos> ObtenerPeriodosActivos()
        {
            try
            {
                return _periodosDAO.ListarActivos();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el servicio al obtener períodos activos: {ex.Message}", ex);
            }
        }

        public List<Periodos> ObtenerPeriodosInactivos()
        {
            try
            {
                // Retorna SOLO períodos activables
                return _periodosDAO.ListarInactivos();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el servicio al obtener períodos inactivos: {ex.Message}", ex);
            }
        }

        // Método para obtener TODOS los períodos inactivos (administración)
        public List<Periodos> ObtenerTodosLosPeriodosInactivos()
        {
            try
            {
                return _periodosDAO.ListarTodosInactivos();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el servicio al obtener todos los períodos inactivos: {ex.Message}", ex);
            }
        }

        public void ActivarPeriodo(int periodoCodigo)
        {
            try
            {
                if (periodoCodigo <= 0)
                {
                    throw new ArgumentException("El código del período debe ser mayor que cero.");
                }

                // Validar antes de activar
                var validacion = _periodosDAO.ValidarActivacionPeriodo(periodoCodigo);
                if (!validacion.PuedeActivar)
                {
                    throw new InvalidOperationException($"No se puede activar el período: {validacion.Razon}");
                }

                bool resultado = _periodosDAO.ActivarPeriodo(periodoCodigo);
                if (!resultado)
                {
                    throw new InvalidOperationException("No se pudo activar el período especificado.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el servicio al activar período: {ex.Message}", ex);
            }
        }

        public void CerrarPeriodoActivo()
        {
            try
            {
                bool resultado = _periodosDAO.CerrarPeriodoActivo();
                if (!resultado)
                {
                    throw new InvalidOperationException("No se pudo cerrar el período activo.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el servicio al cerrar período activo: {ex.Message}", ex);
            }
        }

        public Periodos ObtenerProximoPeriodoActivo()
        {
            try
            {
                return _periodosDAO.ObtenerProximoPeriodoActivo();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el servicio al obtener próximo período: {ex.Message}", ex);
            }
        }

        // Métodos adicionales para validaciones
        public bool PeriodoTienePagos(int periodoCodigo)
        {
            try
            {
                if (periodoCodigo <= 0)
                {
                    return false;
                }

                return _periodosDAO.PeriodoTienePagos(periodoCodigo);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el servicio al verificar pagos del período: {ex.Message}", ex);
            }
        }

        public (bool PuedeActivar, string Razon) ValidarActivacionPeriodo(int periodoCodigo)
        {
            try
            {
                if (periodoCodigo <= 0)
                {
                    return (false, "El código del período debe ser mayor que cero.");
                }

                return _periodosDAO.ValidarActivacionPeriodo(periodoCodigo);
            }
            catch (Exception ex)
            {
                return (false, $"Error en la validación: {ex.Message}");
            }
        }

        public Periodos ObtenerUltimoPeriodoPagado()
        {
            try
            {
                return _periodosDAO.ObtenerUltimoPeriodoPagado();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el servicio al obtener último período pagado: {ex.Message}", ex);
            }
        }
    }
}