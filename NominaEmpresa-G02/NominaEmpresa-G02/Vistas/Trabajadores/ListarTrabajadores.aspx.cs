using NominaEmpresa.Aplicacion;
using System;

namespace NominaEmpresa.Presentacion
{
    public partial class ListarTrabajadores : System.Web.UI.Page
    {
        private TrabajadoresAplicacion _trabajadoresAplicacion;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _trabajadoresAplicacion = new TrabajadoresAplicacion();
                CargarTrabajadores();
            }
        }

        private void CargarTrabajadores()
        {
            var listaTrabajadores = _trabajadoresAplicacion.ObtenerTrabajadoresActivos();
            gvTrabajadores.DataSource = listaTrabajadores;
            gvTrabajadores.DataBind();
        }
    }
}
