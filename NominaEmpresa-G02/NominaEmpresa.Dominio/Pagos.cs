using System;
using System.ComponentModel.DataAnnotations;

namespace NominaEmpresa.Dominio
{
    public class Pagos
    {
        // Propiedades principales
        public int PagosCodigo { get; set; }
        public int ContratoCodigo { get; set; }
        public int TrabajadoresCodigo { get; set; }
        public int PeriodoCodigo { get; set; }
        public DateTime PagosFechaPago { get; set; }
        public decimal PagosSalarioNeto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        // Propiedades adicionales devueltas por el SP
        public string NombreTrabajador { get; set; } // Nombre completo del trabajador
        public string PeriodoNombre { get; set; } // Nombre del período
        public decimal SueldoBase { get; set; } // Sueldo base del contrato

        // Relaciones virtuales
        public virtual Trabajadores Trabajador { get; set; }
        public virtual ContratoLaborales Contratos { get; set; }
        public virtual Periodos Periodo { get; set; }

        // Constructor vacío
        public Pagos()
        {
        }

        // Constructor lleno (sin propiedades adicionales)
        public Pagos(
            int pagosCodigo,
            int contratoCodigo,
            int trabajadoresCodigo,
            int periodoCodigo,
            DateTime pagosFechaPago,
            decimal pagosSalarioNeto,
            DateTime fechaCreacion,
            DateTime fechaModificacion)
        {
            PagosCodigo = pagosCodigo;
            ContratoCodigo = contratoCodigo;
            TrabajadoresCodigo = trabajadoresCodigo;
            PeriodoCodigo = periodoCodigo;
            PagosFechaPago = pagosFechaPago;
            PagosSalarioNeto = pagosSalarioNeto;
            FechaCreacion = fechaCreacion;
            FechaModificacion = fechaModificacion;
        }

        // Constructor completo (incluye propiedades adicionales)
        public Pagos(
            int pagosCodigo,
            int contratoCodigo,
            int trabajadoresCodigo,
            int periodoCodigo,
            DateTime pagosFechaPago,
            decimal pagosSalarioNeto,
            DateTime fechaCreacion,
            DateTime fechaModificacion,
            string nombreTrabajador,
            string periodoNombre,
            decimal sueldoBase)
        {
            PagosCodigo = pagosCodigo;
            ContratoCodigo = contratoCodigo;
            TrabajadoresCodigo = trabajadoresCodigo;
            PeriodoCodigo = periodoCodigo;
            PagosFechaPago = pagosFechaPago;
            PagosSalarioNeto = pagosSalarioNeto;
            FechaCreacion = fechaCreacion;
            FechaModificacion = fechaModificacion;
            NombreTrabajador = nombreTrabajador;
            PeriodoNombre = periodoNombre;
            SueldoBase = sueldoBase;
        }
    }
}
