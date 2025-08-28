using System;
using System.Collections.Generic;
using NominaEmpresa.Dominio;
using NominaEmpresa.Persistencia;

namespace NominaEmpresa.Servicios
{
    public class PagosServicio
    {
        private readonly PagosDAO _pagosDAO;

        public PagosServicio()
        {
            _pagosDAO = new PagosDAO();
        }

        public (bool Exitoso, string Mensaje) RealizarPago()
        {
            try
            {
                string resultado = _pagosDAO.RealizarPagoAutomatico();
                return (true, resultado);
            }
            catch (Exception ex)
            {
                return (false, $"Error en el servicio al realizar el pago: {ex.Message}");
            }
        }

        public List<Pagos> ObtenerTodosPagos()
        {
            try
            {
                return _pagosDAO.ListarPagosConDesglose();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el servicio al obtener los pagos: {ex.Message}", ex);
            }
        }

        public Pagos ObtenerDetallePago(int pagosCodigo)
        {
            try
            {
                if (pagosCodigo <= 0)
                {
                    throw new ArgumentException("El código del pago debe ser mayor que cero.");
                }

                return _pagosDAO.ObtenerDetallePago(pagosCodigo);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el servicio al obtener el detalle del pago: {ex.Message}", ex);
            }
        }

        public Dictionary<string, decimal> ObtenerResumenPorPeriodo()
        {
            try
            {
                return _pagosDAO.ObtenerResumenPorPeriodo();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el servicio al obtener el resumen por período: {ex.Message}", ex);
            }
        }

        // Método para validar que un pago se puede generar
        public bool PuedeGenerarPago(int contratoCodigo, int periodoCodigo)
        {
            try
            {
                // Aquí puedes agregar lógica de validación
                // Por ejemplo: verificar que no exista ya un pago para ese contrato en ese período
                return true; // Simplificado por ahora
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al validar si se puede generar el pago: {ex.Message}", ex);
            }
        }

        // Método para obtener totales generales
        public Dictionary<string, decimal> ObtenerTotalesGenerales()
        {
            try
            {
                var pagos = _pagosDAO.ListarPagosConDesglose();
                var totales = new Dictionary<string, decimal>
                {
                    ["TotalPagos"] = pagos.Count,
                    ["TotalSueldoBase"] = 0,
                    ["TotalBonos"] = 0,
                    ["TotalDeducciones"] = 0,
                    ["TotalHorasExtras"] = 0,
                    ["TotalNeto"] = 0
                };

                foreach (var pago in pagos)
                {
                    totales["TotalSueldoBase"] += pago.SueldoBase;
                    totales["TotalBonos"] += pago.BonosCalculados ?? 0;
                    totales["TotalDeducciones"] += pago.DeduccionesCalculadas ?? 0;
                    totales["TotalHorasExtras"] += pago.HorasExtrasCalculadas ?? 0;
                    totales["TotalNeto"] += pago.PagosSalarioNeto;
                }

                return totales;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener totales generales: {ex.Message}", ex);
            }
        }
    }
}