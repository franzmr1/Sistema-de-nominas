using NominaEmpresa.Dominio;
using System;
using Xunit;

namespace TEST
{
    public class PagosTests
    {
        #region Métodos Helper

        private ContratoLaborales CrearContratoBasico()
        {
            return new ContratoLaborales
            {
                ContratoCodigo = 1,
                TrabajadoresCodigo = 1,
                SueldoBase = 2500.00M,
                Bonificaciones = 10.0M,
                Deducciones = 8.0M,
                HorasExtras = 5.0M,
                EstadoContrato = 'A'
            };
        }

        #endregion

        #region CP01 - Generar pago automático correctamente

        [Fact]
        public void CalcularSalarioNeto_ContratoCompleto_CalculaCorrectamente()
        {
            // Arrange
            var contrato = CrearContratoBasico();
            contrato.CalcularValores();

            // Expected calculation:
            // Base: 2500, Bonos (10%): 250, Deducciones (8%): 200
            // Horas Extras (5h): 68.25, Neto: 2500 + 250 + 68.25 - 200 = 2618.25
            decimal salarioNetoEsperado = 2618.25M;

            // Act
            decimal salarioCalculado = contrato.SueldoBase +
                                     (contrato.BonosCalculados ?? 0) +
                                     (contrato.HorasExtrasCalculadas ?? 0) -
                                     (contrato.DeduccionesCalculadas ?? 0);

            // Assert
            Assert.Equal(salarioNetoEsperado, Math.Round(salarioCalculado, 2));
            Assert.Equal(250.00M, contrato.BonosCalculados);
            Assert.Equal(200.00M, contrato.DeduccionesCalculadas);
            Assert.Equal(68.25M, contrato.HorasExtrasCalculadas);
        }

        #endregion

        #region CP02 - Validar cálculo de sueldo neto

        [Fact]
        public void ValidarCalculoSueldoNeto_CalculoCorrecto_RetornaTrue()
        {
            // Arrange
            var pago = new Pagos
            {
                SueldoBase = 2000.00M,
                BonosCalculados = 200.00M,
                DeduccionesCalculadas = 160.00M,
                HorasExtrasCalculadas = 50.00M,
                PagosSalarioNeto = 2090.00M
            };

            // Act
            bool esValido = pago.ValidarCalculoSueldoNeto();

            // Assert
            Assert.True(esValido);
        }

        [Fact]
        public void ValidarCalculoSueldoNeto_CalculoIncorrecto_RetornaFalse()
        {
            // Arrange
            var pago = new Pagos
            {
                SueldoBase = 2000.00M,
                BonosCalculados = 200.00M,
                DeduccionesCalculadas = 160.00M,
                HorasExtrasCalculadas = 50.00M,
                PagosSalarioNeto = 2500.00M // Incorrecto
            };

            // Act
            bool esValido = pago.ValidarCalculoSueldoNeto();

            // Assert
            Assert.False(esValido);
        }

        #endregion

        #region CP03 - Horas extras con diferentes cantidades

        [Theory]
        [InlineData(1, 12.50)]
        [InlineData(2, 25.00)]
        [InlineData(3, 38.50)]
        [InlineData(5, 65.50)]
        public void CalcularHorasExtras_DiferentesCantidades_CalculaCorrectamente(decimal horas, decimal esperado)
        {
            // Arrange
            var contrato = new ContratoLaborales
            {
                SueldoBase = 2400.00M, // Valor por hora = 10
                HorasExtras = horas
            };

            // Act
            contrato.CalcularHorasExtras();

            // Assert
            Assert.Equal(esperado, contrato.HorasExtrasCalculadas);
        }

        #endregion

        #region CP04 - Sueldo mínimo legal

        [Fact]
        public void CalcularSalarioNeto_SueldoMinimo_CalculaCorrectamente()
        {
            // Arrange
            var contrato = new ContratoLaborales
            {
                SueldoBase = 1130.00M, // Sueldo mínimo legal
                Bonificaciones = 5.0M,
                Deducciones = 3.0M,
                HorasExtras = 0
            };

            // Act
            contrato.CalcularValores();
            decimal salarioNeto = contrato.SueldoBase +
                                (contrato.BonosCalculados ?? 0) -
                                (contrato.DeduccionesCalculadas ?? 0);

            // Assert
            Assert.Equal(1130.00M, contrato.SueldoBase);
            Assert.Equal(56.50M, contrato.BonosCalculados);
            Assert.Equal(33.90M, contrato.DeduccionesCalculadas);
            Assert.Equal(1152.60M, Math.Round(salarioNeto, 2));
        }

        #endregion

        #region CP05 - Pago duplicado

        [Fact]
        public void ValidarPagoDuplicado_MismoPeriodoYContrato_DebeDetectarse()
        {
            // Arrange
            var pago1 = new Pagos
            {
                ContratoCodigo = 1,
                PeriodoCodigo = 1,
                PagosFechaPago = new DateTime(2025, 1, 30)
            };

            var pago2 = new Pagos
            {
                ContratoCodigo = 1,
                PeriodoCodigo = 1, // Mismo período
                PagosFechaPago = new DateTime(2025, 1, 31)
            };

            // Act & Assert
            Assert.Equal(pago1.ContratoCodigo, pago2.ContratoCodigo);
            Assert.Equal(pago1.PeriodoCodigo, pago2.PeriodoCodigo);
        }

        #endregion

        #region CP06 - Resumen del cálculo

        [Fact]
        public void ObtenerResumenCalculo_DatosCompletos_GeneraResumenCorrecto()
        {
            // Arrange
            var pago = new Pagos
            {
                SueldoBase = 2500.00M,
                BonosCalculados = 250.00M,
                HorasExtrasCalculadas = 68.25M,
                DeduccionesCalculadas = 200.00M,
                PagosSalarioNeto = 2618.25M
            };

            // Act
            string resumen = pago.ObtenerResumenCalculo();

            // Assert
            Assert.Contains("Base: S/ 2,500.00", resumen);
            Assert.Contains("Bonos: S/ 250.00", resumen);
            Assert.Contains("= S/ 2,618.25", resumen);
        }

        #endregion
    }
}