using NominaEmpresa.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NominaEmpresa.Persistencia
{
    public class PeriodosDAO
    {
        private readonly Conexion conexion;

        public PeriodosDAO()
        {
            conexion = new Conexion();
        }

        public List<Periodos> ListarActivos()
        {
            List<Periodos> listaPeriodos = new List<Periodos>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spPeriodosListarActivos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Periodos periodo = new Periodos
                        {
                            PeriodoCodigo = Convert.ToInt32(reader["PeriodoCodigo"]),
                            PeriodoNombre = reader["PeriodoNombre"].ToString(),
                            PeriodoActivo = Convert.ToBoolean(reader["PeriodoActivo"])
                        };
                        listaPeriodos.Add(periodo);
                    }
                }
            }

            return listaPeriodos;
        }

        public List<Periodos> ListarInactivos()
        {
            List<Periodos> listaPeriodos = new List<Periodos>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                // Cambiar a SP que solo retorna períodos activables
                SqlCommand cmd = new SqlCommand("spListarPeriodosActivables", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Periodos periodo = new Periodos
                        {
                            PeriodoCodigo = Convert.ToInt32(reader["PeriodoCodigo"]),
                            PeriodoNombre = reader["PeriodoNombre"].ToString(),
                            PeriodoFechaInicio = Convert.ToDateTime(reader["PeriodoFechaInicio"]),
                            PeriodoFechaFin = Convert.ToDateTime(reader["PeriodoFechaFin"]),
                            PeriodoActivo = Convert.ToBoolean(reader["PeriodoActivo"])
                        };
                        listaPeriodos.Add(periodo);
                    }
                }
            }

            return listaPeriodos;
        }

        public bool ActivarPeriodo(int periodoCodigo)
        {
            try
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    // Usar stored procedure con validaciones
                    SqlCommand cmd = new SqlCommand("spPeriodosActivarConValidaciones", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PeriodoCodigo", periodoCodigo);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al activar el período: " + ex.Message);
            }
        }

        public bool CerrarPeriodoActivo()
        {
            try
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("spPeriodosCerrarActivo", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al cerrar el período activo: " + ex.Message);
            }
        }

        // Método adicional para obtener TODOS los períodos inactivos (para administración)
        public List<Periodos> ListarTodosInactivos()
        {
            List<Periodos> listaPeriodos = new List<Periodos>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spPeriodosListarInactivos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Periodos periodo = new Periodos
                        {
                            PeriodoCodigo = Convert.ToInt32(reader["PeriodoCodigo"]),
                            PeriodoNombre = reader["PeriodoNombre"].ToString(),
                            PeriodoActivo = Convert.ToBoolean(reader["PeriodoActivo"])
                        };
                        listaPeriodos.Add(periodo);
                    }
                }
            }

            return listaPeriodos;
        }

        public Periodos ObtenerProximoPeriodoActivo()
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spObtenerProximoPeriodoDisponible", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Periodos
                        {
                            PeriodoCodigo = Convert.ToInt32(reader["PeriodoCodigo"]),
                            PeriodoNombre = reader["PeriodoNombre"].ToString(),
                            PeriodoFechaInicio = Convert.ToDateTime(reader["PeriodoFechaInicio"]),
                            PeriodoFechaFin = Convert.ToDateTime(reader["PeriodoFechaFin"]),
                            PeriodoActivo = Convert.ToBoolean(reader["PeriodoActivo"])
                        };
                    }
                }
            }

            return null;
        }

        // Nuevos métodos agregados para validaciones
        public bool PeriodoTienePagos(int periodoCodigo)
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
                throw new Exception("Error al verificar pagos del período: " + ex.Message);
            }
        }

        public (bool PuedeActivar, string Razon) ValidarActivacionPeriodo(int periodoCodigo)
        {
            try
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("spValidarActivacionPeriodo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PeriodoCodigo", periodoCodigo);

                    // Parámetros de salida
                    SqlParameter puedeActivar = new SqlParameter("@PuedeActivar", SqlDbType.Bit);
                    puedeActivar.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(puedeActivar);

                    SqlParameter razon = new SqlParameter("@Razon", SqlDbType.NVarChar, 500);
                    razon.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(razon);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    bool puede = Convert.ToBoolean(puedeActivar.Value);
                    string motivoRazon = razon.Value?.ToString() ?? "";

                    return (puede, motivoRazon);
                }
            }
            catch (SqlException ex)
            {
                return (false, "Error al validar período: " + ex.Message);
            }
        }

        public Periodos ObtenerUltimoPeriodoPagado()
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spObtenerUltimoPeriodoPagado", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Periodos
                        {
                            PeriodoCodigo = Convert.ToInt32(reader["PeriodoCodigo"]),
                            PeriodoNombre = reader["PeriodoNombre"].ToString(),
                            PeriodoFechaInicio = Convert.ToDateTime(reader["PeriodoFechaInicio"]),
                            PeriodoFechaFin = Convert.ToDateTime(reader["PeriodoFechaFin"]),
                            PeriodoActivo = Convert.ToBoolean(reader["PeriodoActivo"])
                        };
                    }
                }
            }

            return null;
        }
    }
}