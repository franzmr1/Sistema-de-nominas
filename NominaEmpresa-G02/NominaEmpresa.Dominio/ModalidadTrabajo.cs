using System;
namespace NominaEmpresa.Dominio
{
    public class ModalidadTrabajo
    {
        public int ModalidadCodigo { get; set; }
        public string ModalidadNombre { get; set; }
        public string Descripcion { get; set; }

        // Constructor vacío
        public ModalidadTrabajo() { }

        // Constructor con parámetros
        public ModalidadTrabajo(int codigo, string nombre, string descripcion)
        {
            ModalidadCodigo = codigo;
            ModalidadNombre = nombre;
            Descripcion = descripcion;
        }
    }
}