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
        // Método para obtener trabajadores activos
        public List<Trabajadores> ObtenerTrabajadoresActivos()
        {
            return _trabajadoresServicio.ObtenerTrabajadoresActivos();
        }
        // Método para obtener trabajadores inactivos
        public List<Trabajadores> ObtenerTrabajadoresInactivos()
        {
            return _trabajadoresServicio.ObtenerTrabajadoresInactivos();
        }
        // Método para obtener un trabajador por ID
        public Trabajadores ObtenerTrabajadorPorId(int trabajadorCodigo)
        {
            return _trabajadoresServicio.ObtenerTrabajadorPorId(trabajadorCodigo);
        }
        // Método para agregar un nuevo trabajador
        public void AgregarTrabajador(Trabajadores trabajador)
        {
            _trabajadoresServicio.AgregarTrabajador(trabajador);
        }
        // Método para actualizar un trabajador
        public void ActualizarTrabajador(Trabajadores trabajador)
        {
            _trabajadoresServicio.ActualizarTrabajador(trabajador);
        }
        // Método para eliminar (inhabilitar) un trabajador
        public void EliminarTrabajador(int trabajadorCodigo)
        {
            _trabajadoresServicio.EliminarTrabajador(trabajadorCodigo);
        }
        // Método para recuperar un trabajador inactivo
        public void RecuperarTrabajador(int trabajadorCodigo)
        {
            _trabajadoresServicio.RecuperarTrabajador(trabajadorCodigo);
        }
    }
}