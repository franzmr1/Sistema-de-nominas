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

        public (bool Exitoso, string Mensaje) RealizarPago()
        {
            try
            {
                return _pagosServicio.RealizarPago();
            }
            catch (Exception ex)
            {
                return (false, $"Error en la aplicación al realizar el pago automático: {ex.Message}");
            }
        }

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

        // Nuevo método para obtener detalle específico
        public Pagos ObtenerDetallePago(int pagosCodigo)
        {
            try
            {
                return _pagosServicio.ObtenerDetallePago(pagosCodigo);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la aplicación al obtener el detalle del pago: {ex.Message}", ex);
            }
        }

        // Método para obtener resumen de pagos
        public Dictionary<string, object> ObtenerResumenPagos()
        {
            try
            {
                var pagos = _pagosServicio.ObtenerTodosPagos();
                var resumen = new Dictionary<string, object>();

                if (pagos != null && pagos.Count > 0)
                {
                    decimal totalPagado = 0;
                    decimal totalSueldoBase = 0;
                    decimal totalBonos = 0;
                    decimal totalDeducciones = 0;
                    decimal totalHorasExtras = 0;

                    foreach (var pago in pagos)
                    {
                        totalPagado += pago.PagosSalarioNeto;
                        totalSueldoBase += pago.SueldoBase;
                        totalBonos += pago.BonosCalculados ?? 0;
                        totalDeducciones += pago.DeduccionesCalculadas ?? 0;
                        totalHorasExtras += pago.HorasExtrasCalculadas ?? 0;
                    }

                    resumen["TotalPagos"] = pagos.Count;
                    resumen["TotalPagado"] = totalPagado;
                    resumen["TotalSueldoBase"] = totalSueldoBase;
                    resumen["TotalBonos"] = totalBonos;
                    resumen["TotalDeducciones"] = totalDeducciones;
                    resumen["TotalHorasExtras"] = totalHorasExtras;
                    resumen["PromedioSueldo"] = totalPagado / pagos.Count;
                }
                else
                {
                    resumen["TotalPagos"] = 0;
                    resumen["TotalPagado"] = 0;
                    resumen["TotalSueldoBase"] = 0;
                    resumen["TotalBonos"] = 0;
                    resumen["TotalDeducciones"] = 0;
                    resumen["TotalHorasExtras"] = 0;
                    resumen["PromedioSueldo"] = 0;
                }

                return resumen;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la aplicación al obtener el resumen de pagos: {ex.Message}", ex);
            }
        }

        // Método para validar cálculos de nómina
        public (bool EsValido, List<string> Errores) ValidarCalculosNomina()
        {
            var errores = new List<string>();
            bool esValido = true;

            try
            {
                var pagos = _pagosServicio.ObtenerTodosPagos();

                foreach (var pago in pagos)
                {
                    if (!pago.ValidarCalculoSueldoNeto())
                    {
                        errores.Add($"Error en cálculo del pago #{pago.PagosCodigo} para {pago.NombreTrabajador}");
                        esValido = false;
                    }
                }

                if (errores.Count == 0)
                {
                    errores.Add("Todos los cálculos de nómina son correctos.");
                }
            }
            catch (Exception ex)
            {
                errores.Add($"Error al validar cálculos: {ex.Message}");
                esValido = false;
            }

            return (esValido, errores);
        }

        // Método para obtener estadísticas por período
        public Dictionary<string, decimal> ObtenerEstadisticasPorPeriodo()
        {
            try
            {
                return _pagosServicio.ObtenerResumenPorPeriodo();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en la aplicación al obtener estadísticas por período: {ex.Message}", ex);
            }
        }
    }
}