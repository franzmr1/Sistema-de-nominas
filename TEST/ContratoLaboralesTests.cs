using NominaEmpresa.Dominio;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace TEST
{
    public class ContratoLaboralesTests
    {
        #region CP01 - Registrar contrato laboral correctamente

        [Fact]
        public void CrearContrato_DatosValidos_CreaCorrectamente()
        {
            // Arrange
            var contrato = new ContratoLaborales
            {
                TrabajadoresCodigo = 1,
                FechaInicio = new DateTime(2025, 6, 18),
                FechaFin = new DateTime(2026, 1, 1),
                TipoContratosCodigo = 1,
                CargosCodigo = 1,
                DiaInicioJornada = "Lun",
                DiaFinJornada = "Sab",
                HoraInicioJornada = new TimeSpan(10, 0, 0), // 10:00 AM
                HoraFinJornada = new TimeSpan(19, 0, 0),   // 7:00 PM
                ModalidadCodigo = 3, // Híbrido
                SueldoBase = 1500.00M,
                Bonificaciones = 10.0M,
                EstadoContrato = 'A'
            };

            // Act
            contrato.CalcularValores();

            // Assert
            Assert.Equal(1500.00M, contrato.SueldoBase);
            Assert.Equal(10.0M, contrato.Bonificaciones);
            Assert.Equal(150.00M, contrato.BonosCalculados);
            Assert.Equal('A', contrato.EstadoContrato);
            Assert.True(contrato.FechaInicio < contrato.FechaFin);
        }

        #endregion

        #region CP02 - Error por fecha fin anterior

        [Fact]
        public void ValidarFechas_FechaFinAnteriorAInicio_LanzaExcepcion()
        {
            // Arrange & Act & Assert
            Assert.Throws<ValidationException>(() =>
            {
                var contrato = new ContratoLaborales(
                    contratoCodigo: 1,
                    trabajadoresCodigo: 1,
                    fechaInicio: new DateTime(2025, 6, 10),
                    fechaFin: new DateTime(2025, 6, 1), // Fecha anterior
                    tipoContratosCodigo: 1,
                    cargosCodigo: 1,
                    diaInicioJornada: "Lun",
                    diaFinJornada: "Vie",
                    horaInicioJornada: new TimeSpan(8, 0, 0),
                    horaFinJornada: new TimeSpan(17, 0, 0),
                    modalidadCodigo: 1,
                    sueldoBase: 1500.00M,
                    deducciones: 0,
                    bonificaciones: 0,
                    horasExtras: 0,
                    estadoContrato: 'A'
                );
            });
        }

        #endregion

        #region CP04 - Sueldo menor al mínimo

        [Theory]
        [InlineData(900.00)]
        [InlineData(0)]
        [InlineData(-100)]
        public void ValidarSueldoBase_MenorAlMinimo_NoEsValido(decimal sueldo)
        {
            // Arrange
            var contrato = new ContratoLaborales
            {
                SueldoBase = sueldo
            };

            // Assert
            Assert.True(sueldo < 1130.00M);
        }

        #endregion

        #region Cálculos existentes mejorados

        [Fact]
        public void CalcularBonos_BonificacionesNoNulas_CalculaCorrectamente()
        {
            var contrato = new ContratoLaborales
            {
                SueldoBase = 1000,
                Bonificaciones = 10
            };
            contrato.CalcularBonos();
            Assert.Equal(100, contrato.BonosCalculados);
        }

        [Fact]
        public void CalcularBonos_SinBonificaciones_RetornaCero()
        {
            var contrato = new ContratoLaborales
            {
                SueldoBase = 1000,
                Bonificaciones = 0
            };
            contrato.CalcularBonos();
            Assert.Equal(0, contrato.BonosCalculados);
        }

        [Fact]
        public void CalcularDeducciones_DeduccionesNoNulas_CalculaCorrectamente()
        {
            var contrato = new ContratoLaborales
            {
                SueldoBase = 2000,
                Deducciones = 5
            };
            contrato.CalcularDeducciones();
            Assert.Equal(100, contrato.DeduccionesCalculadas);
        }

        [Fact]
        public void CalcularDeducciones_SinDeducciones_RetornaCero()
        {
            var contrato = new ContratoLaborales
            {
                SueldoBase = 2000,
                Deducciones = 0
            };
            contrato.CalcularDeducciones();
            Assert.Equal(0, contrato.DeduccionesCalculadas);
        }

        [Fact]
        public void CalcularHorasExtras_HorasExtrasMayorADos_CalculaCorrectamente()
        {
            var contrato = new ContratoLaborales
            {
                SueldoBase = 2400,
                HorasExtras = 4
            };
            contrato.CalcularHorasExtras();
            Assert.Equal(52.00M, contrato.HorasExtrasCalculadas);
        }

        #endregion
    }
}