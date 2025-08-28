using System.Data.SqlClient;

namespace NominaEmpresa.Persistencia
{
    public class Conexion
    {
        private readonly string cadenaConexion;

        public Conexion()
        {
            cadenaConexion = "Server=DESKTOP-VIU264U\\SQLEXPRESS;Database=BDNominaEmpresaV2;Trusted_Connection=True;";
        }
        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(cadenaConexion);
        }
    }
}
