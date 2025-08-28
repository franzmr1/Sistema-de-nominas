using NominaEmpresa.Dominio;
using NominaEmpresa.Persistencia;
using System;
using System.Collections.Generic;

namespace NominaEmpresa.Servicios
{
    public class PagosServicio
    {
        private readonly PagosDAO _pagosDAO;

        public PagosServicio()
        {
            _pagosDAO = new PagosDAO();
        }

        /// <summary>
        /// Realiza el proceso de pago para el período activo
        /// </summary>
        /// <returns>True si el pago se realizó exitosamente, False en caso contrario</returns>
        public (bool Exitoso, string Mensaje) RealizarPago()
        {
            try
            {
                string resultado = _pagosDAO.RealizarPago();
                bool exitoso = resultado.Contains("Pagos realizados exitosamente");
                return (exitoso, resultado);
            }
            catch (Exception ex)
            {
                // Log del error si tienes un sistema de logging
                return (false, $"Error al realizar el pago: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene todos los pagos registrados en el sistema
        /// </summary>
        /// <returns>Lista de pagos</returns>
        /// <exception cref="Exception">Se lanza si hay un error al obtener los pagos</exception>
        public List<Pagos> ObtenerTodosPagos()
        {
            try
            {
                return _pagosDAO.ListarPagos();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los pagos: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene los pagos correspondientes a un período específico
        /// </summary>
        /// <param name="periodoCodigo">Código del período a consultar</param>
        /// <returns>Lista de pagos del período especificado</returns>
        /// <exception cref="ArgumentException">Se lanza si el código del período es inválido</exception>
        /// <exception cref="Exception">Se lanza si hay un error al obtener los pagos</exception>
        public List<Pagos> ObtenerPagosPorPeriodo(int periodoCodigo)
        {
            try
            {
                if (periodoCodigo <= 0)
                {
                    throw new ArgumentException("El código del período debe ser mayor que cero", nameof(periodoCodigo));
                }

                return _pagosDAO.ListarPagosPorPeriodo(periodoCodigo);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los pagos del período {periodoCodigo}: {ex.Message}", ex);
            }
        }

    }
}