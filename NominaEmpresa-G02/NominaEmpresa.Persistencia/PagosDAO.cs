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

        private T GetValueOrDefault<T>(SqlDataReader reader, string columnName)
        {
            int columnIndex = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(columnIndex))
            {
                return default(T);
            }
            return (T)Convert.ChangeType(reader[columnName], typeof(T));
        }

        public string RealizarPago()
        {
            try
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("sp_RealizarPago", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["Mensaje"].ToString();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al realizar el pago: " + ex.Message);
            }

            return "No se pudo realizar ningún pago.";
        }

        public List<Pagos> ListarPagos()
        {
            List<Pagos> listaPagos = new List<Pagos>();

            try
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("sp_ListarPagos", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pago = new Pagos
                            {
                                PagosCodigo = GetValueOrDefault<int>(reader, "CodigoPago"),
                                PeriodoNombre = GetValueOrDefault<string>(reader, "Periodo"),
                                NombreTrabajador = GetValueOrDefault<string>(reader, "NombreTrabajador"),
                                PagosFechaPago = GetValueOrDefault<DateTime>(reader, "FechaPago"),
                                SueldoBase = GetValueOrDefault<decimal>(reader, "SueldoBase"),
                                PagosSalarioNeto = GetValueOrDefault<decimal>(reader, "SalarioNeto")
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
                            try
                            {
                                var pago = new Pagos
                                {
                                    PagosCodigo = GetValueOrDefault<int>(reader, "CodigoPago"),
                                    PeriodoCodigo = periodoCodigo,
                                    PagosFechaPago = GetValueOrDefault<DateTime>(reader, "FechaPago"),
                                    PagosSalarioNeto = GetValueOrDefault<decimal>(reader, "SalarioNeto")
                                };

                                listaPagos.Add(pago);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error al procesar el registro del período {periodoCodigo}: {ex.Message}");
                                continue;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al listar los pagos del período {periodoCodigo}: " + ex.Message);
            }

            return listaPagos;
        }
    }
}