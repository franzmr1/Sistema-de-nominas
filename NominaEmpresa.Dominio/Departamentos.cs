
using System;
namespace NominaEmpresa.Dominio
{
    public class Departamentos
    {
        public int DepartamentosCodigo { get; set; }
        public string DepartamentosNombre { get; set; }
        public string Descripcion { get; set; }

        public Departamentos() { }

        public Departamentos(int codigo, string nombre, string descripcion)
        {
            DepartamentosCodigo = codigo;
            DepartamentosNombre = nombre;
            Descripcion = descripcion;
        }
    }
}
