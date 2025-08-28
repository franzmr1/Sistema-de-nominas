using System.Data.SqlClient;

namespace NominaEmpresa.Persistencia
{
    public class Conexion
    {
        private readonly string cadenaConexion;

        public Conexion()
        {
            cadenaConexion = "Server=localhost,1433;Database=BDNomina03;User Id=sa;Password=Example123@Secure!;";
        }
        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }

}
