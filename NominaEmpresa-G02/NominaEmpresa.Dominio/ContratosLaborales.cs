using System;
using System.ComponentModel.DataAnnotations;

namespace NominaEmpresa.Dominio
{
    public class ContratoLaborales
    {
        public int ContratoCodigo { get; set; }

        [Required(ErrorMessage = "El trabajador es obligatorio.")]
        public int TrabajadoresCodigo { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime FechaInicio { get; set; } = DateTime.Now; // Fecha de inicio predeterminada

        public DateTime? FechaFin { get; set; }

        [Required(ErrorMessage = "El tipo de contrato es obligatorio.")]
        public int TipoContratosCodigo { get; set; }

        [Required(ErrorMessage = "El cargo es obligatorio.")]
        public int CargosCodigo { get; set; }

        [Required(ErrorMessage = "El día de inicio de jornada es obligatorio.")]
        public string DiaInicioJornada { get; set; }

        [Required(ErrorMessage = "El día de fin de jornada es obligatorio.")]
        public string DiaFinJornada { get; set; }

        [Required(ErrorMessage = "La hora de inicio de jornada es obligatoria.")]
        public TimeSpan HoraInicioJornada { get; set; }

        [Required(ErrorMessage = "La hora de fin de jornada es obligatoria.")]
        public TimeSpan HoraFinJornada { get; set; }

        [Required(ErrorMessage = "La modalidad de trabajo es obligatoria.")]
        public int ModalidadCodigo { get; set; }

        [Required(ErrorMessage = "El sueldo es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El sueldo debe ser un número positivo.")]
        public decimal SueldoBase { get; set; }

        // Campos no obligatorios
        public decimal? Deducciones { get; set; }
        public decimal? Bonificaciones { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Las horas extras deben ser un valor positivo.")]
        public decimal? HorasExtras { get; set; }

        public char? EstadoContrato { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        // Propiedades de navegación
        public virtual Trabajadores Trabajador { get; set; }
        public virtual TipoContratos TipoContrato { get; set; }
        public virtual Cargos Cargo { get; set; }
        public virtual ModalidadTrabajo Modalidad { get; set; }

        // Constructor vacío
        public ContratoLaborales()
        {
            FechaInicio = DateTime.Now;
            FechaCreacion = DateTime.Now;
            FechaModificacion = DateTime.Now;
        }

        // Constructor con parámetros
        public ContratoLaborales(int contratoCodigo, int trabajadoresCodigo, DateTime fechaInicio,
            DateTime? fechaFin, int tipoContratosCodigo, int cargosCodigo,
            string diaInicioJornada, string diaFinJornada,
            TimeSpan horaInicioJornada, TimeSpan horaFinJornada,
            int modalidadCodigo, decimal sueldoBase, decimal? deducciones, decimal? bonificaciones,
            decimal? horasExtras, char? estadoContrato)
        {
            ContratoCodigo = contratoCodigo;
            TrabajadoresCodigo = trabajadoresCodigo;
            FechaInicio = fechaInicio == default ? DateTime.Now : fechaInicio; // Asignar fecha actual si está vacía
            FechaFin = fechaFin;
            TipoContratosCodigo = tipoContratosCodigo;
            CargosCodigo = cargosCodigo;
            DiaInicioJornada = diaInicioJornada;
            DiaFinJornada = diaFinJornada;

            HoraInicioJornada = horaInicioJornada != TimeSpan.Zero ? horaInicioJornada : throw new ValidationException("La hora de inicio de jornada no puede ser 00:00.");
            HoraFinJornada = horaFinJornada != TimeSpan.Zero ? horaFinJornada : throw new ValidationException("La hora de fin de jornada no puede ser 00:00.");

            ModalidadCodigo = modalidadCodigo;
            SueldoBase = sueldoBase;
            Deducciones = deducciones;
            Bonificaciones = bonificaciones;
            HorasExtras = horasExtras;
            EstadoContrato = estadoContrato;
            FechaCreacion = DateTime.Now;
            FechaModificacion = DateTime.Now;
        }
    }
}
