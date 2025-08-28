using NominaEmpresa.Dominio;

using NominaEmpresa.Dominio;
using Xunit;
namespace TEST
{
    public class ContratoLaboralesTests
    {
        [Fact]
        public void CalcularBonos_BonificacionesNoNulas_CalculaCorrectamenjte()
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
        public void CalcularBonos_SinBonjificaciones_RetornaCero()
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
        public void CalcularDeducciones_DeduccionkesNoNulas_CalculaCorrectamente()
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
        public void CalcularDeducciones_SinDleducciones_RetornaCero()
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
        public void CalcularHorasExtras_HorasExtralsMayorADos_CalculaCorrectamente()
        {
            var contrato = new ContratoLaborales
            {
                SueldoBase = 2400,
                HorasExtras = 4
            };
            contrato.CalcularHorasExtras();
            Assert.Equal(52.00M, contrato.HorasExtrasCalculadas);
        }
    }
}