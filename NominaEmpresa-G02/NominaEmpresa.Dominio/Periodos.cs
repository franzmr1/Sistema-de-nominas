using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NominaEmpresa.Dominio
{
    public class Periodos
    {
        // Propiedades
        public int PeriodoCodigo { get; set; }
        public string PeriodoNombre { get; set; }
        public DateTime PeriodoFechaInicio { get; set; }
        public DateTime PeriodoFechaFin { get; set; }
        public bool PeriodoActivo { get; set; }

        // Constructor vacío
        public Periodos()
        {
        }

        // Constructor lleno
        public Periodos(int periodoCodigo, string periodoNombre, DateTime periodoFechaInicio, DateTime periodoFechaFin, bool periodoActivo)
        {
            PeriodoCodigo = periodoCodigo;
            PeriodoNombre = periodoNombre;
            PeriodoFechaInicio = periodoFechaInicio;
            PeriodoFechaFin = periodoFechaFin;
            PeriodoActivo = periodoActivo;
        }
    }
}
