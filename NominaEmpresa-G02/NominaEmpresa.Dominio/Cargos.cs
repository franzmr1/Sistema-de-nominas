// Cargos.cs
using System;
namespace NominaEmpresa.Dominio
{
    public class Cargos
    {
        public int CargosCodigo { get; set; }
        public string CargosNombre { get; set; }
        public string Descripcion { get; set; }
        public int DepartamentosCodigo { get; set; }
        public virtual Departamentos Departamento { get; set; }

        // Constructor vacío
        public Cargos() { }

        // Constructor con parámetros
        public Cargos(int codigo, string nombre, string descripcion, int departamentoCodigo)
        {
            CargosCodigo = codigo;
            CargosNombre = nombre;
            Descripcion = descripcion;
            DepartamentosCodigo = departamentoCodigo;
        }
    }
}