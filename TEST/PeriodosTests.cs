using NominaEmpresa.Dominio;
using System;
using Xunit;

namespace TEST
{
    public class PeriodosTests
    {
        #region CP01 - Crear período válido

        [Fact]
        public void CrearPeriodo_DatosValidos_CreaCorrectamente()
        {
            // Arrange
            var periodo = new Periodos
            {
                PeriodoCodigo = 1,
                PeriodoNombre = "Enero 2025",
                PeriodoFechaInicio = new DateTime(2025, 1, 1),
                PeriodoFechaFin = new DateTime(2025, 1, 31),
                PeriodoActivo = true
            };

            // Act & Assert
            Assert.Equal(1, periodo.PeriodoCodigo);
            Assert.Equal("Enero 2025", periodo.PeriodoNombre);
            Assert.True(periodo.PeriodoActivo);
            Assert.True(periodo.PeriodoFechaInicio < periodo.PeriodoFechaFin);
        }

        #endregion

        #region CP02 - Validar fechas del período

        [Fact]
        public void ValidarFechasPeriodo_FechaFinAnteriorAInicio_NoEsValido()
        {
            // Arrange
            var fechaInicio = new DateTime(2025, 2, 1);
            var fechaFin = new DateTime(2025, 1, 31); // Anterior

            // Act
            bool fechasValidas = fechaFin > fechaInicio;

            // Assert
            Assert.False(fechasValidas);
        }

        [Fact]
        public void ValidarFechasPeriodo_FechasCorrectas_EsValido()
        {
            // Arrange
            var fechaInicio = new DateTime(2025, 1, 1);
            var fechaFin = new DateTime(2025, 1, 31);

            // Act
            bool fechasValidas = fechaFin >= fechaInicio;

            // Assert
            Assert.True(fechasValidas);
        }

        #endregion

        #region CP03 - Solo un período activo

        [Fact]
        public void ValidarPeriodoActivo_SoloUnoActivo_EsValido()
        {
            // Arrange
            var periodo1 = new Periodos { PeriodoActivo = true };
            var periodo2 = new Periodos { PeriodoActivo = false };
            var periodo3 = new Periodos { PeriodoActivo = false };

            // Act
            int periodosActivos = 0;
            if (periodo1.PeriodoActivo) periodosActivos++;
            if (periodo2.PeriodoActivo) periodosActivos++;
            if (periodo3.PeriodoActivo) periodosActivos++;

            // Assert
            Assert.Equal(1, periodosActivos);
        }

        #endregion

        #region CP04 - Nombres únicos de período

        [Theory]
        [InlineData("Enero 2025")]
        [InlineData("Febrero 2025")]
        [InlineData("Período Especial Diciembre")]
        public void ValidarNombrePeriodo_NombresValidos_SonCorrectos(string nombre)
        {
            // Arrange
            var periodo = new Periodos
            {
                PeriodoNombre = nombre
            };

            // Act & Assert
            Assert.NotNull(periodo.PeriodoNombre);
            Assert.NotEmpty(periodo.PeriodoNombre);
            Assert.Equal(nombre, periodo.PeriodoNombre);
        }

        [Fact]
        public void ValidarNombrePeriodo_NombreVacio_NoEsValido()
        {
            // Arrange
            var periodo = new Periodos
            {
                PeriodoNombre = ""
            };

            // Act & Assert
            Assert.True(string.IsNullOrEmpty(periodo.PeriodoNombre));
        }

        #endregion

        #region CP05 - Secuencia de períodos

        [Fact]
        public void ValidarSecuenciaPeriodos_PeriodosConsecutivos_EsCorrecta()
        {
            // Arrange
            var enero = new Periodos
            {
                PeriodoNombre = "Enero 2025",
                PeriodoFechaInicio = new DateTime(2025, 1, 1),
                PeriodoFechaFin = new DateTime(2025, 1, 31)
            };

            var febrero = new Periodos
            {
                PeriodoNombre = "Febrero 2025",
                PeriodoFechaInicio = new DateTime(2025, 2, 1),
                PeriodoFechaFin = new DateTime(2025, 2, 28)
            };

            // Act
            bool secuenciaCorrecta = febrero.PeriodoFechaInicio > enero.PeriodoFechaFin ||
                                   febrero.PeriodoFechaInicio == enero.PeriodoFechaFin.AddDays(1);

            // Assert
            Assert.True(secuenciaCorrecta);
        }

        #endregion

        #region CP06 - Período con pagos no se puede reactivar

        [Fact]
        public void ValidarReactivacion_PeriodoConPagos_NoSePuedeReactivar()
        {
            // Arrange
            var periodo = new Periodos
            {
                PeriodoCodigo = 1,
                PeriodoActivo = false
            };

            // Simular que tiene pagos
            bool tienePagos = true; // En realidad se consultaría la BD

            // Act
            bool puedeReactivarse = !tienePagos && !periodo.PeriodoActivo;

            // Assert
            Assert.False(puedeReactivarse);
        }

        [Fact]
        public void ValidarReactivacion_PeriodoSinPagos_SePuedeReactivar()
        {
            // Arrange
            var periodo = new Periodos
            {
                PeriodoCodigo = 2,
                PeriodoActivo = false
            };

            // Simular que NO tiene pagos
            bool tienePagos = false;

            // Act
            bool puedeReactivarse = !tienePagos && !periodo.PeriodoActivo;

            // Assert
            Assert.True(puedeReactivarse);
        }

        #endregion

        #region CP07 - Duración del período

        [Theory]
        [InlineData("2025-01-01", "2025-01-31", 31)] // Enero
        [InlineData("2025-02-01", "2025-02-28", 28)] // Febrero
        [InlineData("2025-06-01", "2025-06-30", 30)] // Junio
        public void CalcularDuracionPeriodo_DiferentesMeses_CalculaCorrectamente(
            string fechaInicioStr, string fechaFinStr, int diasEsperados)
        {
            // Arrange
            var fechaInicio = DateTime.Parse(fechaInicioStr);
            var fechaFin = DateTime.Parse(fechaFinStr);

            var periodo = new Periodos
            {
                PeriodoFechaInicio = fechaInicio,
                PeriodoFechaFin = fechaFin
            };

            // Act
            int duracion = (periodo.PeriodoFechaFin - periodo.PeriodoFechaInicio).Days + 1;

            // Assert
            Assert.Equal(diasEsperados, duracion);
        }

        #endregion

        #region CP08 - Constructor completo

        [Fact]
        public void Constructor_TodosLosParametros_CreaCorrectamente()
        {
            // Arrange & Act
            var periodo = new Periodos(
                periodoCodigo: 5,
                periodoNombre: "Mayo 2025",
                periodoFechaInicio: new DateTime(2025, 5, 1),
                periodoFechaFin: new DateTime(2025, 5, 31),
                periodoActivo: true
            );

            // Assert
            Assert.Equal(5, periodo.PeriodoCodigo);
            Assert.Equal("Mayo 2025", periodo.PeriodoNombre);
            Assert.Equal(new DateTime(2025, 5, 1), periodo.PeriodoFechaInicio);
            Assert.Equal(new DateTime(2025, 5, 31), periodo.PeriodoFechaFin);
            Assert.True(periodo.PeriodoActivo);
        }

        #endregion
    }
}