using Services;
using static System.Console;

namespace Tests
{
    public class PsychrometricCalculatorTests
    {
        [Theory]
        [InlineData(32, 10, 2.64)]
        [InlineData(42, 10, 3.93)]
        [InlineData(52, 10, 5.75)]
        [InlineData(62, 10, 8.30)]
        [InlineData(72, 10, 11.82)]
        [InlineData(77, 70, 98.34)]
        public void CalculateGppTest(int temp, int hum, double gpp)
        {
            var calc = new PsychrometricCalculator();

            WriteLine(calc.CalculateGpp(temp, hum));
            Assert.Equal(gpp, calc.CalculateGpp(temp, hum));
        }
    }
}
