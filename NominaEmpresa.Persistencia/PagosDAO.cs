using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NominaEmpresa.Dominio;

namespace NominaEmpresa.Persistencia
{
    public class PagosDAO
    {
        private readonly Conexion conexion;

        public PagosDAO()
        {
            conexion = new Conexion();
        }

        #region Métodos Helper para SqlDataReader

        private string GetSafeString(SqlDataReader reader, string columnName)
        {
            try
            {
                int columnIndex = reader.GetOrdinal(columnName);
                return reader.IsDBNull(columnIndex) ? null : reader.GetString(columnIndex);
            }
            catch (ArgumentException)
            {
                // La columna no existe
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private decimal GetSafeDecimal(SqlDataReader reader, string columnName, decimal defaultValue = 0)
        {
            try
            {
                int columnIndex = reader.GetOrdinal(columnName);
                return reader.IsDBNull(columnIndex) ? defaultValue : reader.GetDecimal(columnIndex);
            }
            catch (ArgumentException)
            {
                // La columna no existe
                return defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        private decimal? GetSafeNullableDecimal(SqlDataReader reader, string columnName)
        {
            try
            {
                int columnIndex = reader.GetOrdinal(columnName);
                return reader.IsDBNull(columnIndex) ? (decimal?)null : reader.GetDecimal(columnIndex);
            }
            catch (ArgumentException)
            {
                // La columna no existe
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private int GetSafeInt(SqlDataReader reader, string columnName, int defaultValue = 0)
        {
            try
            {
                int columnIndex = reader.GetOrdinal(columnName);
                return reader.IsDBNull(columnIndex) ? defaultValue : reader.GetInt32(columnIndex);
            }
            catch (ArgumentException)
            {
                // La columna no existe
                return defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        private int? GetSafeNullableInt(SqlDataReader reader, string columnName)
        {
            try
            {
                int columnIndex = reader.GetOrdinal(columnName);
                return reader.IsDBNull(columnIndex) ? (int?)null : reader.GetInt32(columnIndex);
            }
            catch (ArgumentException)
            {
                // La columna no existe
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private DateTime GetSafeDateTime(SqlDataReader reader, string columnName, DateTime defaultValue = default)
        {
            try
            {
                int columnIndex = reader.GetOrdinal(columnName);
                return reader.IsDBNull(columnIndex) ? defaultValue : reader.GetDateTime(columnIndex);
            }
            catch (ArgumentException)
            {
                // La columna no existe
                return defaultValue == default ? DateTime.Now : defaultValue;
            }
            catch (Exception)
            {
                return defaultValue == default ? DateTime.Now : defaultValue;
            }
        }

        private DateTime? GetSafeNullableDateTime(SqlDataReader reader, string columnName)
        {
            try
            {
                int columnIndex = reader.GetOrdinal(columnName);
                return reader.IsDBNull(columnIndex) ? (DateTime?)null : reader.GetDateTime(columnIndex);
            }
            catch (ArgumentException)
            {
                // La columna no existe
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Realiza el pago automático ejecutando el stored procedure
        /// </summary>
        /// <returns>Mensaje de confirmación</returns>
        public string RealizarPagoAutomatico()
        {
            try
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("sp_RealizarPagoAutomatico", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 60; // 60 segundos de timeout
                    con.Open();
                    cmd.ExecuteNonQuery();

                    return "Pagos realizados con éxito.";
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al realizar los pagos automáticos: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al realizar pagos: " + ex.Message);
            }
        }

        /// <summary>
        /// Lista todos los pagos con desglose completo del cálculo
        /// </summary>
        /// <returns>Lista de pagos con información detallada</returns>
        public List<Pagos> ListarPagosConDesglose()
        {
            List<Pagos> listaPagos = new List<Pagos>();
            try
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("spListarPagosCompleto", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 60;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pago = new Pagos
                            {
                                // Campos obligatorios
                                PagosCodigo = GetSafeInt(reader, "PagosCodigo"),
                                ContratoCodigo = GetSafeInt(reader, "ContratoCodigo"),
                                TrabajadoresCodigo = GetSafeInt(reader, "TrabajadoresCodigo"),
                                PeriodoCodigo = GetSafeInt(reader, "PeriodoCodigo"),
                                PagosFechaPago = GetSafeDateTime(reader, "PagosFechaPago"),
                                PagosSalarioNeto = GetSafeDecimal(reader, "PagosSalarioNeto"),
                                FechaCreacion = GetSafeDateTime(reader, "FechaCreacion"),
                                FechaModificacion = GetSafeDateTime(reader, "FechaModificacion"),

                                // Información del trabajador y período
                                NombreTrabajador = GetSafeString(reader, "NombreTrabajador"),
                                PeriodoNombre = GetSafeString(reader, "NombrePeriodo"),
                                SueldoBase = GetSafeDecimal(reader, "SueldoBase"),

                                // Desglose del cálculo - campos nullable
                                PorcentajeBonificaciones = GetSafeNullableDecimal(reader, "PorcentajeBonificaciones"),
                                BonosCalculados = GetSafeNullableDecimal(reader, "BonosCalculados"),
                                PorcentajeDeducciones = GetSafeNullableDecimal(reader, "PorcentajeDeducciones"),
                                DeduccionesCalculadas = GetSafeNullableDecimal(reader, "DeduccionesCalculadas"),
                                HorasExtras = GetSafeNullableDecimal(reader, "HorasExtras"),
                                HorasExtrasCalculadas = GetSafeNullableDecimal(reader, "HorasExtrasCalculadas"),

                                // Información adicional del contrato
                                CargoNombre = GetSafeString(reader, "CargoNombre"),
                                TipoContratoNombre = GetSafeString(reader, "TipoContratoNombre"),
                                ModalidadNombre = GetSafeString(reader, "ModalidadNombre")
                            };

                            listaPagos.Add(pago);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error de SQL al listar los pagos con desglose: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al listar pagos: " + ex.Message);
            }
            return listaPagos;
        }

        /// <summary>
        /// Método original para compatibilidad con código existente
        /// </summary>
        /// <returns>Lista de pagos</returns>
        public List<Pagos> ListarPagos()
        {
            List<Pagos> listaPagos = new List<Pagos>();
            try
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("spListarPagos", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pago = new Pagos
                            {
                                PagosCodigo = GetSafeInt(reader, "PagosCodigo"),
                                ContratoCodigo = GetSafeInt(reader, "ContratoCodigo"),
                                NombreTrabajador = GetSafeString(reader, "NombreTrabajador"),
                                SueldoBase = GetSafeDecimal(reader, "SueldoBase"),
                                PagosSalarioNeto = GetSafeDecimal(reader, "PagosSalarioNeto"),
                                PeriodoNombre = GetSafeString(reader, "NombrePeriodo")
                            };
                            listaPagos.Add(pago);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al listar los pagos: " + ex.Message);
            }
            return listaPagos;
        }

        /// <summary>
        /// Obtiene el detalle completo de un pago específico
        /// </summary>
        /// <param name="pagosCodigo">Código del pago</param>
        /// <returns>Información detallada del pago</returns>
        public Pagos ObtenerDetallePago(int pagosCodigo)
        {
            try
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("spObtenerDetallePago", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PagosCodigo", pagosCodigo);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Pagos
                            {
                                // Información básica del pago
                                PagosCodigo = GetSafeInt(reader, "PagosCodigo"),
                                ContratoCodigo = GetSafeInt(reader, "ContratoCodigo"),
                                TrabajadoresCodigo = GetSafeInt(reader, "TrabajadoresCodigo"),
                                PeriodoCodigo = GetSafeInt(reader, "PeriodoCodigo"),
                                PagosFechaPago = GetSafeDateTime(reader, "PagosFechaPago"),
                                PagosSalarioNeto = GetSafeDecimal(reader, "PagosSalarioNeto"),
                                FechaCreacion = GetSafeDateTime(reader, "FechaCreacion"),
                                FechaModificacion = GetSafeDateTime(reader, "FechaModificacion"),

                                // Información del trabajador
                                NombreTrabajador = GetSafeString(reader, "NombreTrabajador"),

                                // Información del período
                                PeriodoNombre = GetSafeString(reader, "NombrePeriodo"),

                                // Información del contrato
                                SueldoBase = GetSafeDecimal(reader, "SueldoBase"),

                                // Desglose del cálculo
                                PorcentajeBonificaciones = GetSafeNullableDecimal(reader, "PorcentajeBonificaciones"),
                                BonosCalculados = GetSafeNullableDecimal(reader, "BonosCalculados"),
                                PorcentajeDeducciones = GetSafeNullableDecimal(reader, "PorcentajeDeducciones"),
                                DeduccionesCalculadas = GetSafeNullableDecimal(reader, "DeduccionesCalculadas"),
                                HorasExtras = GetSafeNullableDecimal(reader, "HorasExtras"),
                                HorasExtrasCalculadas = GetSafeNullableDecimal(reader, "HorasExtrasCalculadas"),

                                // Información adicional
                                CargoNombre = GetSafeString(reader, "CargoNombre"),
                                TipoContratoNombre = GetSafeString(reader, "TipoContratoNombre"),
                                ModalidadNombre = GetSafeString(reader, "ModalidadNombre")
                            };
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error de SQL al obtener el detalle del pago: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al obtener detalle del pago: " + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Obtiene un resumen de pagos agrupados por período
        /// </summary>
        /// <returns>Diccionario con período y total pagado</returns>
        public Dictionary<string, decimal> ObtenerResumenPorPeriodo()
        {
            Dictionary<string, decimal> resumen = new Dictionary<string, decimal>();
            try
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("spResumenPagosPorPeriodo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string periodo = GetSafeString(reader, "PeriodoNombre");
                            decimal total = GetSafeDecimal(reader, "TotalPagado");

                            if (!string.IsNullOrEmpty(periodo))
                            {
                                resumen[periodo] = total;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error de SQL al obtener resumen por período: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al obtener resumen: " + ex.Message);
            }
            return resumen;
        }

        /// <summary>
        /// Lista pagos por período específico
        /// </summary>
        /// <param name="periodoCodigo">Código del período</param>
        /// <returns>Lista de pagos del período</returns>
        public List<Pagos> ListarPagosPorPeriodo(int periodoCodigo)
        {
            List<Pagos> listaPagos = new List<Pagos>();
            try
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("sp_ListarPagosPorPeriodo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PeriodoCodigo", periodoCodigo);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pago = new Pagos
                            {
                                PagosCodigo = GetSafeInt(reader, "PagosCodigo"),
                                ContratoCodigo = GetSafeInt(reader, "ContratoCodigo"),
                                NombreTrabajador = GetSafeString(reader, "NombreTrabajador"),
                                SueldoBase = GetSafeDecimal(reader, "SueldoBase"),
                                PagosSalarioNeto = GetSafeDecimal(reader, "PagosSalarioNeto"),
                                PeriodoNombre = GetSafeString(reader, "NombrePeriodo"),
                                PagosFechaPago = GetSafeDateTime(reader, "PagosFechaPago")
                            };
                            listaPagos.Add(pago);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error de SQL al listar pagos por período: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al listar pagos por período: " + ex.Message);
            }
            return listaPagos;
        }

        /// <summary>
        /// Verifica si ya existen pagos para un período específico
        /// </summary>
        /// <param name="periodoCodigo">Código del período</param>
        /// <returns>True si ya existen pagos</returns>
        public bool ExistenPagosParaPeriodo(int periodoCodigo)
        {
            try
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Pagos WHERE PeriodoCodigo = @PeriodoCodigo", con);
                    cmd.Parameters.AddWithValue("@PeriodoCodigo", periodoCodigo);
                    con.Open();

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error de SQL al verificar existencia de pagos: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al verificar pagos: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene estadísticas generales de pagos
        /// </summary>
        /// <returns>Diccionario con estadísticas</returns>
        public Dictionary<string, object> ObtenerEstadisticasGenerales()
        {
            Dictionary<string, object> estadisticas = new Dictionary<string, object>();
            try
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    string query = @"
                        SELECT 
                            COUNT(*) as TotalPagos,
                            SUM(PagosSalarioNeto) as TotalMonto,
                            AVG(PagosSalarioNeto) as PromedioSueldo,
                            MIN(PagosFechaPago) as PrimeraFecha,
                            MAX(PagosFechaPago) as UltimaFecha
                        FROM Pagos";

                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            estadisticas["TotalPagos"] = GetSafeInt(reader, "TotalPagos");
                            estadisticas["TotalMonto"] = GetSafeDecimal(reader, "TotalMonto");
                            estadisticas["PromedioSueldo"] = GetSafeDecimal(reader, "PromedioSueldo");
                            estadisticas["PrimeraFecha"] = GetSafeNullableDateTime(reader, "PrimeraFecha");
                            estadisticas["UltimaFecha"] = GetSafeNullableDateTime(reader, "UltimaFecha");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error de SQL al obtener estadísticas: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general al obtener estadísticas: " + ex.Message);
            }
            return estadisticas;
        }

        #endregion
    }
}