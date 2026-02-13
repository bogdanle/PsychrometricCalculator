using Services;
using static System.Console;

namespace TestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var calc = new PsychrometricCalculator();

            WriteLine(calc.CalculateGpp(42, 10, 0));
            WriteLine(calc.CalculateGpp(42, 10, 50));
            WriteLine(calc.CalculateGpp(42, 10, 500));
            WriteLine(calc.CalculateGpp(42, 10, 1000));
            WriteLine(calc.CalculateGpp(42, 10, 1500));
        }
    }
}
