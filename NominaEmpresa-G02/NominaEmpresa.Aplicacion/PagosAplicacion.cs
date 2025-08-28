using NominaEmpresa.Dominio;
using NominaEmpresa.Servicios;
using System;
using System.Collections.Generic;

namespace NominaEmpresa.Aplicacion
{
    public class PagosAplicacion
    {
        private readonly PagosServicio _pagosServicio;

        public PagosAplicacion()
        {
            _pagosServicio = new PagosServicio();
        }

        /// <summary>
        /// Realiza el proceso de pago para el período activo
        /// </summary>
        /// <returns>Tupla con el resultado de la operación y mensaje informativo</returns>
        public (bool Exitoso, string Mensaje) RealizarPago()
        {
            try
            {
                return _pagosServicio.RealizarPago();
            }
            catch (Exception ex)
            {
                return (false, $"Error en la aplicación al realizar el pago: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene todos los pagos registrados en el sistema
        /// </summary>
        /// <returns>Lista de pagos</returns>
        public List<Pagos> ObtenerTodosPagos()
        {
            try
            {
                return _pagosServicio.ObtenerTodosPagos();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la aplicación al obtener todos los pagos: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene los pagos correspondientes a un período específico
        /// </summary>
        /// <param name="periodoCodigo">Código del período a consultar</param>
        /// <returns>Lista de pagos del período especificado</returns>
        public List<Pagos> ObtenerPagosPorPeriodo(int periodoCodigo)
        {
            try
            {
                return _pagosServicio.ObtenerPagosPorPeriodo(periodoCodigo);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Error en la aplicación: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la aplicación al obtener los pagos del período {periodoCodigo}: {ex.Message}", ex);
            }
        }
    }
}