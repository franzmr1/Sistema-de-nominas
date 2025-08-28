using NominaEmpresa.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NominaEmpresa.Persistencia
{
    public class TrabajadoresDAO
    {
        private readonly Conexion conexion;

        public TrabajadoresDAO()
        {
            conexion = new Conexion();
        }

        // Listar Trabajadores Activos
        public List<Trabajadores> ListarActivos()
        {
            List<Trabajadores> listaTrabajadores = new List<Trabajadores>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spTrabajadoresListarActivos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Trabajadores trabajador = new Trabajadores
                        {
                            TrabajadoresCodigo = Convert.ToInt32(reader["TrabajadoresCodigo"]),
                            TrabajadoresNombre = reader["TrabajadoresNombreCompleto"].ToString(),
                            TrabajadoresDocumentoIdentidad = reader["TrabajadoresDocumentoIdentidad"].ToString(),
                            TrabajadoresTelefono = reader["TrabajadoresTelefono"].ToString(),
                            TrabajadoresEmail = reader["TrabajadoresEmail"].ToString()
                        };
                        listaTrabajadores.Add(trabajador);
                    }
                }
            }

            return listaTrabajadores;
        }

        // Listar Trabajadores Inactivos
        public List<Trabajadores> ListarInactivos()
        {
            List<Trabajadores> listaTrabajadores = new List<Trabajadores>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spTrabajadoresListarInactivos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Trabajadores trabajador = new Trabajadores
                        {
                            TrabajadoresCodigo = Convert.ToInt32(reader["TrabajadoresCodigo"]),   
                            TrabajadoresApellido = reader["TrabajadoresNombreCompleto"].ToString(),
                            TrabajadoresDocumentoIdentidad = reader["TrabajadoresDocumentoIdentidad"].ToString(),
                            TrabajadoresTelefono = reader["TrabajadoresTelefono"].ToString(),
                            TrabajadoresEmail = reader["TrabajadoresEmail"].ToString()
                        };
                        listaTrabajadores.Add(trabajador);
                    }
                }
            }

            return listaTrabajadores;
        }

        // Obtener Trabajador por ID
        public Trabajadores ObtenerTrabajadorPorId(int trabajadorCodigo)
        {
            Trabajadores trabajador = null;

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spTrabajadorPorId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TrabajadoresCodigo", trabajadorCodigo);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        trabajador = new Trabajadores
                        {
                            TrabajadoresCodigo = Convert.ToInt32(reader["TrabajadoresCodigo"]),
                            TrabajadoresNombre = reader["TrabajadoresNombre"].ToString(),  // Extraer directamente el nombre
                            TrabajadoresApellido = reader["TrabajadoresApellido"].ToString(),  // Extraer directamente el apellido
                            TrabajadoresFechaNacimiento = Convert.ToDateTime(reader["TrabajadoresFechaNacimiento"]),
                            TrabajadoresDocumentoIdentidad = reader["TrabajadoresDocumentoIdentidad"].ToString(),
                            TrabajadoresDireccion = reader["TrabajadoresDireccion"].ToString(),
                            TrabajadoresTelefono = reader["TrabajadoresTelefono"].ToString(),
                            TrabajadoresEmail = reader["TrabajadoresEmail"].ToString(),
                            TrabajadoresDiscapacidad = Convert.ToBoolean(reader["TrabajadoresDiscapacidad"]),
                            TrabajadoresEstado = Convert.ToChar(reader["TrabajadoresEstado"]),
                            FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                            FechaModificacion = Convert.ToDateTime(reader["FechaModificacion"])
                        };
                    }
                }
            }

            return trabajador;
        }




        // Insertar Trabajador
        public void InsertarTrabajador(Trabajadores trabajador)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spTrabajadoresInsertar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TrabajadoresNombre", trabajador.TrabajadoresNombre);
                cmd.Parameters.AddWithValue("@TrabajadoresApellido", trabajador.TrabajadoresApellido);
                cmd.Parameters.AddWithValue("@TrabajadoresFechaNacimiento", trabajador.TrabajadoresFechaNacimiento);
                cmd.Parameters.AddWithValue("@TrabajadoresDocumentoIdentidad", trabajador.TrabajadoresDocumentoIdentidad);
                cmd.Parameters.AddWithValue("@TrabajadoresDireccion", trabajador.TrabajadoresDireccion);
                cmd.Parameters.AddWithValue("@TrabajadoresTelefono", trabajador.TrabajadoresTelefono);
                cmd.Parameters.AddWithValue("@TrabajadoresEmail", trabajador.TrabajadoresEmail);
                cmd.Parameters.AddWithValue("@TrabajadoresDiscapacidad", trabajador.TrabajadoresDiscapacidad);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Actualizar Trabajador
        public void ActualizarTrabajador(Trabajadores trabajador)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spTrabajadoresActualizar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TrabajadoresCodigo", trabajador.TrabajadoresCodigo);
                cmd.Parameters.AddWithValue("@TrabajadoresNombre", trabajador.TrabajadoresNombre);
                cmd.Parameters.AddWithValue("@TrabajadoresApellido", trabajador.TrabajadoresApellido);
                cmd.Parameters.AddWithValue("@TrabajadoresFechaNacimiento", trabajador.TrabajadoresFechaNacimiento);
                cmd.Parameters.AddWithValue("@TrabajadoresDocumentoIdentidad", trabajador.TrabajadoresDocumentoIdentidad);
                cmd.Parameters.AddWithValue("@TrabajadoresDireccion", trabajador.TrabajadoresDireccion);
                cmd.Parameters.AddWithValue("@TrabajadoresTelefono", trabajador.TrabajadoresTelefono);
                cmd.Parameters.AddWithValue("@TrabajadoresEmail", trabajador.TrabajadoresEmail);
                cmd.Parameters.AddWithValue("@TrabajadoresDiscapacidad", trabajador.TrabajadoresDiscapacidad);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Eliminar Trabajador (Cambio de Estado a Inactivo)
        public void EliminarTrabajador(int trabajadorCodigo)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spTrabajadoresEliminar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TrabajadoresCodigo", trabajadorCodigo);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Recuperar Trabajador (Cambio de Estado a Activo)
        public void RecuperarTrabajador(int trabajadorCodigo)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spTrabajadoresRecuperar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TrabajadoresCodigo", trabajadorCodigo);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public Trabajadores ObtenerTrabajadorPorDocumento(string documentoIdentidad)
        {
            Trabajadores trabajador = null;

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spTrabajadorPorDocumento", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TrabajadoresDocumentoIdentidad", documentoIdentidad);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        trabajador = new Trabajadores
                        {
                            TrabajadoresCodigo = Convert.ToInt32(reader["TrabajadoresCodigo"]),
                            TrabajadoresNombre = reader["TrabajadoresNombre"].ToString(),
                            TrabajadoresApellido = reader["TrabajadoresApellido"].ToString(),
                            TrabajadoresDocumentoIdentidad = reader["TrabajadoresDocumentoIdentidad"].ToString(),
                            TrabajadoresTelefono = reader["TrabajadoresTelefono"].ToString(),
                            TrabajadoresEmail = reader["TrabajadoresEmail"].ToString(),
                            TrabajadoresDiscapacidad = Convert.ToBoolean(reader["TrabajadoresDiscapacidad"]),
                            TrabajadoresEstado = Convert.ToChar(reader["TrabajadoresEstado"]),
                            FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                            FechaModificacion = Convert.ToDateTime(reader["FechaModificacion"])
                        };
                    }
                }
            }

            return trabajador;
        }


    }
}
