using System;
using System.ComponentModel.DataAnnotations;

namespace NominaEmpresa.Dominio
{
    public class Pagos
    {
        // Propiedades obligatorias (NOT NULL en BD)
        public int PagosCodigo { get; set; }
        public int ContratoCodigo { get; set; }
        public int TrabajadoresCodigo { get; set; }
        public int PeriodoCodigo { get; set; }
        public DateTime PagosFechaPago { get; set; }
        public decimal PagosSalarioNeto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        // Propiedades adicionales para mostrar información detallada
        public string NombreTrabajador { get; set; }
        public string PeriodoNombre { get; set; }
        public decimal SueldoBase { get; set; }

        // IMPORTANTES: Estas propiedades DEBEN ser nullable (decimal?)
        public decimal? PorcentajeBonificaciones { get; set; }
        public decimal? BonosCalculados { get; set; }
        public decimal? PorcentajeDeducciones { get; set; }
        public decimal? DeduccionesCalculadas { get; set; }
        public decimal? HorasExtras { get; set; }
        public decimal? HorasExtrasCalculadas { get; set; }

        // Propiedades adicionales del empleado y contrato
        public string CargoNombre { get; set; }
        public string TipoContratoNombre { get; set; }
        public string ModalidadNombre { get; set; }

        // Propiedades de navegación
        public virtual Trabajadores Trabajador { get; set; }
        public virtual ContratoLaborales Contratos { get; set; }
        public virtual Periodos Periodo { get; set; }

        public Pagos()
        {
        }

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

        // Constructor completo con desglose
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
            decimal sueldoBase,
            decimal? porcentajeBonificaciones = null,
            decimal? bonosCalculados = null,
            decimal? porcentajeDeducciones = null,
            decimal? deduccionesCalculadas = null,
            decimal? horasExtras = null,
            decimal? horasExtrasCalculadas = null,
            string cargoNombre = null,
            string tipoContratoNombre = null,
            string modalidadNombre = null)
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
            PorcentajeBonificaciones = porcentajeBonificaciones;
            BonosCalculados = bonosCalculados;
            PorcentajeDeducciones = porcentajeDeducciones;
            DeduccionesCalculadas = deduccionesCalculadas;
            HorasExtras = horasExtras;
            HorasExtrasCalculadas = horasExtrasCalculadas;
            CargoNombre = cargoNombre;
            TipoContratoNombre = tipoContratoNombre;
            ModalidadNombre = modalidadNombre;
        }

        // Método para verificar el cálculo del sueldo neto
        public bool ValidarCalculoSueldoNeto()
        {
            decimal calculado = SueldoBase + (BonosCalculados ?? 0) + (HorasExtrasCalculadas ?? 0) - (DeduccionesCalculadas ?? 0);
            return Math.Abs(calculado - PagosSalarioNeto) < 0.01m; // Tolerancia de 1 centavo
        }

        // Método para obtener el resumen del cálculo
        public string ObtenerResumenCalculo()
        {
            return $"Base: {SueldoBase:C2} + Bonos: {BonosCalculados ?? 0:C2} + H.Extra: {HorasExtrasCalculadas ?? 0:C2} - Deduc: {DeduccionesCalculadas ?? 0:C2} = {PagosSalarioNeto:C2}";
        }
    }
}