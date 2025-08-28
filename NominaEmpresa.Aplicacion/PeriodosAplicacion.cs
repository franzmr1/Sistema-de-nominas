using NominaEmpresa.Dominio;
using NominaEmpresa.Servicios;
using System;
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

        public List<Periodos> ObtenerPeriodosActivos()
        {
            try
            {
                return _periodosServicio.ObtenerPeriodosActivos();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la aplicación al obtener períodos activos: {ex.Message}", ex);
            }
        }

        public List<Periodos> ObtenerPeriodosInactivos()
        {
            try
            {
                // Retorna SOLO períodos que pueden activarse
                return _periodosServicio.ObtenerPeriodosInactivos();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la aplicación al obtener períodos inactivos: {ex.Message}", ex);
            }
        }

        // Método adicional para administradores (obtiene TODOS)
        public List<Periodos> ObtenerTodosLosPeriodosInactivos()
        {
            try
            {
                return _periodosServicio.ObtenerTodosLosPeriodosInactivos();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la aplicación al obtener todos los períodos inactivos: {ex.Message}", ex);
            }
        }

        public void ActivarPeriodo(int periodoCodigo)
        {
            try
            {
                // VALIDACIÓN DOBLE: Primero verificamos en el negocio
                var validacion = ValidarActivacionPeriodo(periodoCodigo);
                if (!validacion.PuedeActivar)
                {
                    throw new InvalidOperationException($"Validación de negocio falló: {validacion.Razon}");
                }

                // Verificación adicional: ¿Es realmente el siguiente en secuencia?
                var proximoPeriodo = ObtenerProximoPeriodoActivo();
                if (proximoPeriodo == null || proximoPeriodo.PeriodoCodigo != periodoCodigo)
                {
                    throw new InvalidOperationException("Solo se puede activar el siguiente período en secuencia cronológica.");
                }

                // Si pasa todas las validaciones, proceder
                _periodosServicio.ActivarPeriodo(periodoCodigo);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la aplicación al activar período: {ex.Message}", ex);
            }
        }

        public void CerrarPeriodoActivo()
        {
            try
            {
                _periodosServicio.CerrarPeriodoActivo();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la aplicación al cerrar período activo: {ex.Message}", ex);
            }
        }

        public Periodos ObtenerProximoPeriodoActivo()
        {
            try
            {
                return _periodosServicio.ObtenerProximoPeriodoActivo();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la aplicación al obtener próximo período: {ex.Message}", ex);
            }
        }

        // Métodos adicionales para las validaciones
        public bool PeriodoTienePagos(int periodoCodigo)
        {
            try
            {
                return _periodosServicio.PeriodoTienePagos(periodoCodigo);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la aplicación al verificar pagos del período: {ex.Message}", ex);
            }
        }

        public (bool PuedeActivar, string Razon) ValidarActivacionPeriodo(int periodoCodigo)
        {
            try
            {
                return _periodosServicio.ValidarActivacionPeriodo(periodoCodigo);
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
                return _periodosServicio.ObtenerUltimoPeriodoPagado();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la aplicación al obtener último período pagado: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene períodos inactivos que están disponibles para activar (sin pagos y en secuencia)
        /// </summary>
        public List<Periodos> ObtenerPeriodosInactivosDisponibles()
        {
            try
            {
                var periodosInactivos = _periodosServicio.ObtenerPeriodosInactivos();
                var periodosDisponibles = new List<Periodos>();

                foreach (var periodo in periodosInactivos)
                {
                    var validacion = ValidarActivacionPeriodo(periodo.PeriodoCodigo);
                    if (validacion.PuedeActivar)
                    {
                        periodosDisponibles.Add(periodo);
                    }
                }

                return periodosDisponibles;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la aplicación al obtener períodos inactivos disponibles: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Verifica la integridad de la secuencia de períodos
        /// </summary>
        public (bool SecuenciaCorrecta, List<string> Problemas) VerificarIntegridadSecuencia()
        {
            try
            {
                var problemas = new List<string>();
                bool secuenciaCorrecta = true;

                var periodosActivos = ObtenerPeriodosActivos();
                var periodosInactivos = ObtenerPeriodosInactivos();

                // Verificar que solo hay un período activo
                if (periodosActivos.Count > 1)
                {
                    secuenciaCorrecta = false;
                    problemas.Add("Hay múltiples períodos activos al mismo tiempo");
                }

                return (secuenciaCorrecta, problemas);
            }
            catch (Exception ex)
            {
                return (false, new List<string> { $"Error al verificar integridad: {ex.Message}" });
            }
        }
    }
}