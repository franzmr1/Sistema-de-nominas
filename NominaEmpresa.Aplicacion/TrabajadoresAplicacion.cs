using NominaEmpresa.Dominio;
using NominaEmpresa.Servicios;
using System.Collections.Generic;
namespace NominaEmpresa.Aplicacion
{
    public class TrabajadoresAplicacion
    {
        private readonly TrabajadoresServicio _trabajadoresServicio;
        public TrabajadoresAplicacion()
        {
            _trabajadoresServicio = new TrabajadoresServicio();
        }


        public List<Trabajadores> ObtenerTrabajadoresActivos()
        {
            return _trabajadoresServicio.ObtenerTrabajadoresActivos();
        }
        public List<Trabajadores> ObtenerTrabajadoresInactivos()
        {
            return _trabajadoresServicio.ObtenerTrabajadoresInactivos();
        }
        public Trabajadores ObtenerTrabajadorPorId(int trabajadorCodigo)
        {
            return _trabajadoresServicio.ObtenerTrabajadorPorId(trabajadorCodigo);
        }
        public void AgregarTrabajador(Trabajadores trabajador)
        {
            _trabajadoresServicio.AgregarTrabajador(trabajador);
        }
        public void ActualizarTrabajador(Trabajadores trabajador)
        {
            _trabajadoresServicio.ActualizarTrabajador(trabajador);
        }
        public void EliminarTrabajador(int trabajadorCodigo)
        {
            _trabajadoresServicio.EliminarTrabajador(trabajadorCodigo);
        }
        public void RecuperarTrabajador(int trabajadorCodigo)
        {
            _trabajadoresServicio.RecuperarTrabajador(trabajadorCodigo);
        }
    }

}