// TipoContratos.cs
using System;
namespace NominaEmpresa.Dominio
{
    public class TipoContratos
    {
        public int TipoContratosCodigo { get; set; }
        public string TipoContratosNombre { get; set; }
        public string Descripcion { get; set; }

        // Constructor vacío
        public TipoContratos() { }

        // Constructor con parámetros
        public TipoContratos(int codigo, string nombre, string descripcion)
        {
            TipoContratosCodigo = codigo;
            TipoContratosNombre = nombre;
            Descripcion = descripcion;
        }
    }
}