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
        public DateTime FechaInicio { get; set; } = DateTime.Now;

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

        // Estado del contrato
        public char? EstadoContrato { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        // propiedades calculadas
        public decimal? BonosCalculados { get; set; }
        public decimal? DeduccionesCalculadas { get; set; }
        public decimal? HorasExtrasCalculadas { get; set; }

        // Propiedades de navegación
        public virtual Trabajadores Trabajador { get; set; }
        public virtual TipoContratos TipoContrato { get; set; }
        public virtual Cargos Cargo { get; set; }
        public virtual ModalidadTrabajo Modalidad { get; set; }


        public ContratoLaborales()
        {
            FechaInicio = DateTime.Now;
            FechaCreacion = DateTime.Now;
            FechaModificacion = DateTime.Now;
        }

        public ContratoLaborales(int contratoCodigo, int trabajadoresCodigo, DateTime fechaInicio,
            DateTime? fechaFin, int tipoContratosCodigo, int cargosCodigo,
            string diaInicioJornada, string diaFinJornada,
            TimeSpan horaInicioJornada, TimeSpan horaFinJornada,
            int modalidadCodigo, decimal sueldoBase, decimal? deducciones, decimal? bonificaciones,
            decimal? horasExtras, char? estadoContrato)
        {
            ContratoCodigo = contratoCodigo;
            TrabajadoresCodigo = trabajadoresCodigo;
            FechaInicio = fechaInicio == default ? DateTime.Now : fechaInicio;
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
            ActualizarEstado();
        }

        public void ActualizarEstado()
        {
            if (FechaFin.HasValue && FechaFin.Value < DateTime.Now)
            {
                EstadoContrato = 'I';
            }
        }

        public void CalcularBonos()
        {
            if (Bonificaciones.HasValue && Bonificaciones.Value > 0)
            {
                BonosCalculados = SueldoBase * (Bonificaciones.Value / 100);
            }
            else
            {
                BonosCalculados = 0;
            }
        }

        public void CalcularDeducciones()
        {
            if (Deducciones.HasValue && Deducciones.Value > 0)
            {
                DeduccionesCalculadas = SueldoBase * (Deducciones.Value / 100);
            }
            else
            {
                DeduccionesCalculadas = 0;
            }
        }

        public void CalcularHorasExtras()
        {
            if (HorasExtras.HasValue && HorasExtras.Value > 0)
            {
                decimal horasExtrasTrabajadas = Math.Max(HorasExtras.Value, 0);

                if (SueldoBase <= 0)
                {
                    throw new InvalidOperationException("El sueldo base debe ser mayor que 0.");
                }

                decimal valorPorHora = SueldoBase / 240; 
                valorPorHora = Math.Round(valorPorHora, 2); 

                if (horasExtrasTrabajadas <= 2)
                {
                    HorasExtrasCalculadas = Math.Round(horasExtrasTrabajadas * valorPorHora * 1.25M, 2);
                }
                else
                {
                    decimal horasExtrasAdicionales = horasExtrasTrabajadas - 2;
                    HorasExtrasCalculadas = Math.Round(
                        (2 * valorPorHora * 1.25M) + (horasExtrasAdicionales * valorPorHora * 1.35M), 2);
                }
            }
            else
            {
                HorasExtrasCalculadas = 0;
            }
        }


        public void CalcularValores()
        {
            
            ActualizarEstado();
            CalcularBonos();
            CalcularDeducciones();
            CalcularHorasExtras();
        }
    }
}
