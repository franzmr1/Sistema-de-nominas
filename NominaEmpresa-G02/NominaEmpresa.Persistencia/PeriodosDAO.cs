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

        // Listar Periodos Activos
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
                            PeriodoCodigo = Convert.ToInt32(reader["PeriodoCodigo"]), // Añadido
                            PeriodoNombre = reader["PeriodoNombre"].ToString(),
                            PeriodoActivo = Convert.ToBoolean(reader["PeriodoActivo"])
                        };
                        listaPeriodos.Add(periodo);
                    }
                }
            }

            return listaPeriodos;
        }

        // Listar Periodos Inactivos
        public List<Periodos> ListarInactivos()
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
                            PeriodoCodigo = Convert.ToInt32(reader["PeriodoCodigo"]), // Añadido
                            PeriodoNombre = reader["PeriodoNombre"].ToString(),
                            PeriodoActivo = Convert.ToBoolean(reader["PeriodoActivo"])
                        };
                        listaPeriodos.Add(periodo);
                    }
                }
            }

            return listaPeriodos;
        }

        // Activar Periodo con manejo de excepciones
        public bool ActivarPeriodo(int periodoCodigo)
        {
            try
            {
                using (SqlConnection con = conexion.ObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("spPeriodosActivar", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PeriodoCodigo", periodoCodigo);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                // Aquí puedes manejar el error como prefieras: logging, throw new Exception, etc.
                throw new Exception("Error al activar el periodo: " + ex.Message);
            }
        }

        // Cerrar Periodo Activo con manejo de excepciones
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
                throw new Exception("Error al cerrar el periodo activo: " + ex.Message);
            }
        }
        public void GuardarPeriodo(Periodos periodo)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spPeriodosGuardar", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Si PeriodoCodigo es 0, será una inserción, si no, una actualización
                if (periodo.PeriodoCodigo != 0)
                {
                    cmd.Parameters.AddWithValue("@PeriodoCodigo", periodo.PeriodoCodigo);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PeriodoCodigo", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@Nombre", periodo.PeriodoNombre);
                cmd.Parameters.AddWithValue("@FechaInicio", periodo.PeriodoFechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", periodo.PeriodoFechaFin);
                cmd.Parameters.AddWithValue("@Activo", periodo.PeriodoActivo);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Obtener el próximo periodo cercano al activo
        public Periodos ObtenerProximoPeriodoActivo()
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spProximoPeriodoActivo", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Mapear el resultado al objeto Periodos
                        return new Periodos
                        {
                            PeriodoCodigo = Convert.ToInt32(reader["PeriodoCodigo"]),
                            PeriodoNombre = reader["PeriodoNombre"].ToString(),
                            PeriodoActivo = Convert.ToBoolean(reader["PeriodoActivo"])
                        };
                    }
                }
            }

            // Si no hay resultado, retornar null
            return null;
        }

    }
}