using NominaEmpresa.Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NominaEmpresa.Persistencia
{
    public class ContratosLaboralesDAO
    {
        private readonly Conexion conexion;

        public ContratosLaboralesDAO()
        {
            conexion = new Conexion();
        }
        // Listar Contratos Activos
        public List<ContratoLaborales> ListarActivos()
        {
            List<ContratoLaborales> listaContratos = new List<ContratoLaborales>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spContratosListarActivos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var contrato = new ContratoLaborales
                        {
                            ContratoCodigo = Convert.ToInt32(reader["ContratoCodigo"]),
                            SueldoBase = Convert.ToDecimal(reader["SueldoBase"]),
                            FechaInicio = Convert.ToDateTime(reader["FechaInicio"])
                        };

                        // Manejo de fecha fin nullable
                        if (reader["FechaFin"] != DBNull.Value)
                        {
                            contrato.FechaFin = Convert.ToDateTime(reader["FechaFin"]);
                        }

                        contrato.Trabajador = new Trabajadores
                        {
                            TrabajadoresNombre = reader["TrabajadoresNombreCompleto"].ToString()
                        };
                        contrato.Modalidad = new ModalidadTrabajo
                        {
                            ModalidadNombre = reader["ModalidadNombre"].ToString()
                        };

                        listaContratos.Add(contrato);
                    }
                }
            }

            return listaContratos;
        }

        // Listar Contratos Inactivos
        public List<ContratoLaborales> ListarInactivos()
        {
            List<ContratoLaborales> listaContratos = new List<ContratoLaborales>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spContratosListarInactivos", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var contrato = new ContratoLaborales
                        {
                            ContratoCodigo = Convert.ToInt32(reader["ContratoCodigo"]),
                            SueldoBase = Convert.ToDecimal(reader["SueldoBase"]),
                            FechaInicio = Convert.ToDateTime(reader["FechaInicio"])
                        };

                        // Manejo de fecha fin nullable
                        if (reader["FechaFin"] != DBNull.Value)
                        {
                            contrato.FechaFin = Convert.ToDateTime(reader["FechaFin"]);
                        }

                        contrato.Trabajador = new Trabajadores
                        {
                            TrabajadoresNombre = reader["TrabajadoresNombreCompleto"].ToString()
                        };
                        contrato.Modalidad = new ModalidadTrabajo
                        {
                            ModalidadNombre = reader["ModalidadNombre"].ToString()
                        };

                        listaContratos.Add(contrato);
                    }
                }
            }

            return listaContratos;
        }
        public void ActualizarContrato(ContratoLaborales contrato)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spContratoLaboralActualizar", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ContratoCodigo", contrato.ContratoCodigo);
                cmd.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);

                // Manejo de parámetro FechaFin nullable
                if (contrato.FechaFin.HasValue)
                    cmd.Parameters.AddWithValue("@FechaFin", contrato.FechaFin.Value);
                else
                    cmd.Parameters.AddWithValue("@FechaFin", DBNull.Value);

                cmd.Parameters.AddWithValue("@SueldoBase", contrato.SueldoBase);
                cmd.Parameters.AddWithValue("@TipoContratosCodigo", contrato.TipoContratosCodigo);
                cmd.Parameters.AddWithValue("@CargosCodigo", contrato.CargosCodigo);
                cmd.Parameters.AddWithValue("@DiaInicioJornada", contrato.DiaInicioJornada);
                cmd.Parameters.AddWithValue("@DiaFinJornada", contrato.DiaFinJornada);
                cmd.Parameters.AddWithValue("@HoraInicioJornada", contrato.HoraInicioJornada);
                cmd.Parameters.AddWithValue("@HoraFinJornada", contrato.HoraFinJornada);
                cmd.Parameters.AddWithValue("@ModalidadCodigo", contrato.ModalidadCodigo);

                cmd.Parameters.AddWithValue("@Deducciones", contrato.Deducciones ?? 0.0M);  // Si es null, se pasa 0.0
                cmd.Parameters.AddWithValue("@Bonificaciones", contrato.Bonificaciones ?? 0.0M);  // Si es null, se pasa 0.0
                cmd.Parameters.AddWithValue("@HorasExtras", contrato.HorasExtras ?? 0.0M);  // Si es null, se pasa 0.0


                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        // Eliminar Contrato (Cambio de Estado a Inactivo)
        public void EliminarContrato(int contratoCodigo)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spContratoLaboralEliminar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ContratoCodigo", contratoCodigo);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Recuperar Contrato (Cambio de Estado a Activo)
        public void RecuperarContrato(int contratoCodigo)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spContratoLaboralRecuperar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ContratoCodigo", contratoCodigo);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Métodos para obtener datos de llenado
        public List<TipoContratos> ObtenerTiposContrato(int? codigo = null)
        {
            List<TipoContratos> tipos = new List<TipoContratos>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spTipoContratoObtener", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (codigo.HasValue)
                    cmd.Parameters.AddWithValue("@TipoContratosCodigo", codigo.Value);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tipos.Add(new TipoContratos
                        {
                            TipoContratosCodigo = Convert.ToInt32(reader["TipoContratosCodigo"]),
                            TipoContratosNombre = reader["TipoContratosNombre"].ToString()
                        });
                    }
                }
            }
            return tipos;
        }

        public List<Departamentos> ObtenerDepartamentos(int? codigo = null)
        {
            List<Departamentos> departamentos = new List<Departamentos>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spDepartamentoObtener", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (codigo.HasValue)
                    cmd.Parameters.AddWithValue("@DepartamentosCodigo", codigo.Value);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        departamentos.Add(new Departamentos
                        {
                            DepartamentosCodigo = Convert.ToInt32(reader["DepartamentosCodigo"]),
                            DepartamentosNombre = reader["DepartamentosNombre"].ToString()
                        });
                    }
                }
            }
            return departamentos;
        }

        public List<Cargos> ObtenerCargos(int? codigo = null)
        {
            List<Cargos> cargos = new List<Cargos>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spCargoObtener", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (codigo.HasValue)
                    cmd.Parameters.AddWithValue("@CargosCodigo", codigo.Value);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cargos.Add(new Cargos
                        {
                            CargosCodigo = Convert.ToInt32(reader["CargosCodigo"]),
                            CargosNombre = reader["CargosNombre"].ToString(),
                            DepartamentosCodigo = Convert.ToInt32(reader["DepartamentosCodigo"]),
                            Departamento = new Departamentos
                            {
                                DepartamentosNombre = reader["DepartamentoNombre"].ToString()
                            }
                        });
                    }
                }
            }
            return cargos;
        }

        public List<ModalidadTrabajo> ObtenerModalidades(int? codigo = null)
        {
            List<ModalidadTrabajo> modalidades = new List<ModalidadTrabajo>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spModalidadTrabajoObtener", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (codigo.HasValue)
                    cmd.Parameters.AddWithValue("@ModalidadCodigo", codigo.Value);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        modalidades.Add(new ModalidadTrabajo
                        {
                            ModalidadCodigo = Convert.ToInt32(reader["ModalidadCodigo"]),
                            ModalidadNombre = reader["ModalidadNombre"].ToString()
                        });
                    }
                }
            }

            return modalidades;
        }



        // Método para obtener un contrato por su ID
        public ContratoLaborales ObtenerPorId(int contratoCodigo)
        {
            ContratoLaborales contrato = null;

            using (SqlConnection con = this.conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spContratoPorId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ContratoCodigo", contratoCodigo);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        contrato = new ContratoLaborales
                        {
                            ContratoCodigo = contratoCodigo,
                            TrabajadoresCodigo = Convert.ToInt32(reader["TrabajadoresCodigo"]),
                            FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                            FechaFin = reader["FechaFin"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["FechaFin"]) : null,
                            TipoContratosCodigo = Convert.ToInt32(reader["TipoContratosCodigo"]),
                            CargosCodigo = Convert.ToInt32(reader["CargosCodigo"]),
                            DiaInicioJornada = reader["DiaInicioJornada"].ToString(),
                            DiaFinJornada = reader["DiaFinJornada"].ToString(),
                            HoraInicioJornada = (TimeSpan)reader["HoraInicioJornada"],
                            HoraFinJornada = (TimeSpan)reader["HoraFinJornada"],
                            ModalidadCodigo = Convert.ToInt32(reader["ModalidadCodigo"]),
                            Deducciones = reader["Deducciones"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["Deducciones"]) : null,
                            Bonificaciones = reader["Bonificaciones"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["Bonificaciones"]) : null,
                            HorasExtras = reader["HorasExtras"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["HorasExtras"]) : null,
                            SueldoBase = reader["SueldoBase"] != DBNull.Value ? Convert.ToDecimal(reader["SueldoBase"]) : 0, // Agregado

                            EstadoContrato = Convert.ToChar(reader["EstadoContrato"]),
                            FechaCreacion = Convert.ToDateTime(reader["ContratoFechaCreacion"]),
                            FechaModificacion = Convert.ToDateTime(reader["ContratoFechaModificacion"])
                        };

                        // Inicializar propiedades de navegación
                        contrato.Trabajador = new Trabajadores
                        {
                            TrabajadoresCodigo = Convert.ToInt32(reader["TrabajadoresCodigo"]),
                            TrabajadoresNombre = reader["TrabajadoresNombre"].ToString(),
                            TrabajadoresApellido = reader["TrabajadoresApellido"].ToString(),
                            TrabajadoresFechaNacimiento = reader["TrabajadoresFechaNacimiento"] != DBNull.Value
                                ? (DateTime?)Convert.ToDateTime(reader["TrabajadoresFechaNacimiento"])
                                : null,
                            TrabajadoresDireccion = reader["TrabajadoresDireccion"].ToString(),
                            TrabajadoresTelefono = reader["TrabajadoresTelefono"].ToString(),
                            TrabajadoresEmail = reader["TrabajadoresEmail"].ToString(),
                            TrabajadoresDiscapacidad = Convert.ToBoolean(reader["TrabajadoresDiscapacidad"]),
                            TrabajadoresEstado = Convert.ToChar(reader["TrabajadoresEstado"])
                        };

                        contrato.TipoContrato = new TipoContratos
                        {
                            TipoContratosCodigo = Convert.ToInt32(reader["TipoContratosCodigo"]),
                            TipoContratosNombre = reader["TipoContratosNombre"].ToString(),
                            Descripcion = reader["TipoContratosDescripcion"].ToString()
                        };

                        contrato.Cargo = new Cargos
                        {
                            CargosCodigo = Convert.ToInt32(reader["CargosCodigo"]),
                            CargosNombre = reader["CargosNombre"].ToString(),
                            Descripcion = reader["CargosDescripcion"].ToString(),
                            DepartamentosCodigo = Convert.ToInt32(reader["DepartamentosCodigo"]),
                            Departamento = new Departamentos
                            {
                                DepartamentosCodigo = Convert.ToInt32(reader["DepartamentosCodigo"]),
                                DepartamentosNombre = reader["DepartamentosNombre"].ToString(),
                                Descripcion = reader["DepartamentosDescripcion"].ToString()
                            }
                        };

                        contrato.Modalidad = new ModalidadTrabajo
                        {
                            ModalidadCodigo = Convert.ToInt32(reader["ModalidadCodigo"]),
                            ModalidadNombre = reader["ModalidadNombre"].ToString(),
                            Descripcion = reader["ModalidadDescripcion"].ToString()
                        };
                    }
                }
            }

            return contrato;
        }


        // Método para crear un nuevo contrato
        public int CrearContrato(ContratoLaborales contrato)
        {
            int nuevoContratoCodigo = 1;

            using (SqlConnection con = this.conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spInsertarContrato", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TrabajadoresCodigo", contrato.TrabajadoresCodigo);
                cmd.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", contrato.FechaFin.HasValue ? (object)contrato.FechaFin.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@TipoContratosCodigo", contrato.TipoContratosCodigo);
                cmd.Parameters.AddWithValue("@CargosCodigo", contrato.CargosCodigo);
                cmd.Parameters.AddWithValue("@DiaInicioJornada", contrato.DiaInicioJornada);
                cmd.Parameters.AddWithValue("@DiaFinJornada", contrato.DiaFinJornada);
                cmd.Parameters.AddWithValue("@HoraInicioJornada", contrato.HoraInicioJornada);
                cmd.Parameters.AddWithValue("@HoraFinJornada", contrato.HoraFinJornada);
                cmd.Parameters.AddWithValue("@ModalidadCodigo", contrato.ModalidadCodigo);
                // Para valores decimales, si son nulos o tienen un valor predeterminado de 0.0, se pasan como tal
                cmd.Parameters.AddWithValue("@SueldoBase", contrato.SueldoBase);
                cmd.Parameters.AddWithValue("@Deducciones", contrato.Deducciones ?? 0.0M);  // Si es null, se pasa 0.0
                cmd.Parameters.AddWithValue("@Bonificaciones", contrato.Bonificaciones ?? 0.0M);  // Si es null, se pasa 0.0
                cmd.Parameters.AddWithValue("@HorasExtras", contrato.HorasExtras ?? 0.0M);  // Si es null, se pasa 0.0
                cmd.Parameters.AddWithValue("@EstadoContrato", contrato.EstadoContrato);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        nuevoContratoCodigo = Convert.ToInt32(reader["NuevoContratoCodigo"]);
                    }
                }
            }

            return nuevoContratoCodigo;
        }



        // Método para obtener trabajadores con código y nombre completo
        public List<Trabajadores> ObtenerTrabajadores(int? codigo = null)
        {
            List<Trabajadores> trabajadores = new List<Trabajadores>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spTrabajadorObtener", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (codigo.HasValue)
                    cmd.Parameters.AddWithValue("@TrabajadoresCodigo", codigo.Value);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        trabajadores.Add(new Trabajadores
                        {
                            TrabajadoresCodigo = Convert.ToInt32(reader["TrabajadoresCodigo"]),
                            TrabajadoresNombre = reader["TrabajadoresNombreCompleto"].ToString()
                        });
                    }
                }
            }
            return trabajadores;
        }



        public List<Trabajadores> ObtenerTrabajadoresE(int? trabajadoresCodigo = null)
        {
            List<Trabajadores> trabajadores = new List<Trabajadores>();

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("spTrabajadorObtenerEditar", con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (trabajadoresCodigo.HasValue)
                    cmd.Parameters.AddWithValue("@TrabajadoresCodigo", trabajadoresCodigo.Value);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        trabajadores.Add(new Trabajadores
                        {
                            TrabajadoresCodigo = Convert.ToInt32(reader["TrabajadoresCodigo"]),
                            TrabajadoresNombre = reader["TrabajadoresNombreCompleto"].ToString()
                        });
                    }
                }
            }

            return trabajadores;
        }

    }
}