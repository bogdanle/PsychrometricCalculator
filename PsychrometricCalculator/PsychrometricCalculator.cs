using System;
using Services.Interfaces;

namespace Services
{
    /// <summary>
    /// Functions for calculation of Psychrometric Properties of Moist Air.
    /// Methods used from ASHRAE publication by: R. Wilkinson, P.E. August 1994.
    /// </summary>
    public class PsychrometricCalculator : IPsychrometricCalculator
    {
        public const int Precision = 2;        
        private const double StandardPressureAtSeaLevel = 101325.0; // Pa (Pascals)

        /// <summary>
        /// Function to calculate GPP given dry bulb and relative humidity.
        /// </summary>
        /// <param name="dryBulbTemp">Dry bulb temperature (F).</param>
        /// <param name="relHumidity">Relative humidity (%).</param>
        /// <param name="elevation">Elevation in meters).</param>
        /// <returns>The calculated GPP.</returns>
        public double CalculateGpp(int dryBulbTemp, int relHumidity, int elevation = 0)
        {
            ValidateArguments(dryBulbTemp, relHumidity);

            double pressure = CalculateAtmosphericPressure(elevation);
            double result = CalcHumidityRatio(dryBulbTemp, relHumidity, pressure);
            return Math.Round(result * 7000, Precision);
        }

        /// <summary>
        /// Function to calculate GPP given dry bulb and relative humidity.
        /// </summary>
        /// <param name="dryBulbTemp">Dry bulb temperature (F).</param>
        /// <param name="relHumidity">Relative humidity (%).</param>
        /// <param name="elevation">Elevation in meters).</param>
        /// <returns>The calculated GPP.</returns>
        public double CalculateGpp(double dryBulbTemp, double relHumidity, double elevation = 0)
        {
            ValidateArguments(dryBulbTemp, relHumidity);

            double pressure = CalculateAtmosphericPressure(elevation);
            double result = CalcHumidityRatio(dryBulbTemp, relHumidity);
            return Math.Round(result * 7000, Precision);
        }

        private static void ValidateArguments(int dryBulbTemp, int relHumidity)
        {
            if (relHumidity < 0 || relHumidity > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(relHumidity));
            }

            if (dryBulbTemp < 0 || dryBulbTemp > 120)
            {
                throw new ArgumentOutOfRangeException(nameof(dryBulbTemp));
            }
        }

        private static void ValidateArguments(double dryBulbTemp, double relHumidity)
        {
            if (relHumidity < 0 || relHumidity > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(relHumidity));
            }

            if (dryBulbTemp < 0 || dryBulbTemp > 120)
            {
                throw new ArgumentOutOfRangeException(nameof(dryBulbTemp));
            }
        }

        /// <summary>
        /// Calculate vapor pressure at saturation given dry bulb temp, wet bulb temp, and elevation.
        /// </summary>
        private static double CalcSaturationVaporPressure(double dryBulbTemp)
        {
            const double a1 = -7.90298;
            const double a2 = 5.02808;
            const double a3 = -0.00000013816;
            const double a4 = 11.344;
            const double a5 = 0.0081328;
            const double a6 = -3.49149;
            const double b1 = -9.09718;
            const double b2 = -3.56654;
            const double b3 = 0.876793;
            const double b4 = 0.0060273;

            double p1, p2, p3, p4;

            double ta = (dryBulbTemp + 459.688) / 1.8;
            if (ta > 273.16)
            {
                double z = 373.16 / ta;
                p1 = (z - 1) * a1;
                p2 = Math.Log10(z) * a2;
                p3 = (Math.Pow(10, (1 - 1 / z) * a4) - 1) * a3;
                p4 = (Math.Pow(10, a6 * (z - 1)) - 1) * a5;
            }
            else
            {
                double z = 273.16 / ta;
                p1 = b1 * (z - 1);
                p2 = b2 * Math.Log10(z);
                p3 = b3 * (1 - 1 / z);
                p4 = Math.Log10(b4);
            }

            return 29.921 * Math.Pow(10, p1 + p2 + p3 + p4);
        }

        /// <summary>
        /// Calculate Humidity Ratio given dry bulb temp, relative humidity, and elevation.
        /// </summary>
        private static double CalcHumidityRatio(int dryBulbTemp, int relHum, double atm = 29.921)
        {
            var wsat = CalcSaturationVaporPressure(dryBulbTemp);
            var wtemp = 0.62198 * (wsat / (atm - wsat));
            return relHum * wtemp / 100;
        }

        private static double CalcHumidityRatio(double dryBulbTemp, double relHum, double atm = 29.921)
        {
            var wsat = CalcSaturationVaporPressure(dryBulbTemp);
            var wtemp = 0.62198 * (wsat / (atm - wsat));
            return relHum * wtemp / 100;
        }

        /// <summary>
        /// Calculate atmospheric pressure based on elevation.
        /// Barometric formula: p = p0 * (1 - (L * h) / T0)^(g / (R * L))
        /// Where:
        /// p0 = standard pressure at sea level (Pa)
        /// L = temperature lapse rate (K/m) - typically around 0.0065 K/m
        /// h = elevation (altitude) above sea level (m)
        /// T0 = standard temperature at sea level (K) - typically around 288.15 K
        /// g = acceleration due to gravity (m/s^2) - approximately 9.8 m/s^2
        /// R = specific gas constant for dry air (J/(kg·K)) - approximately 287.05 J/(kg·K)
        /// </summary>
        /// <param name="elevationInMeters">Elevation in meters.</param>
        /// <returns>Atmospheric pressure in inHg.</returns>
        private static double CalculateAtmosphericPressure(double elevationInMeters)
        {           
            double temperatureLapseRate = 0.0065;           // K/m
            double standardTemperatureAtSeaLevel = 288.15;  // K
            double accelerationDueToGravity = 9.8;          // m/s^2
            double specificGasConstant = 287.05;            // J/(kg·K)

            double pressureFactor = Math.Pow(
                                1 - temperatureLapseRate * elevationInMeters / standardTemperatureAtSeaLevel,
                                accelerationDueToGravity / (specificGasConstant * temperatureLapseRate));

            double atmosphericPressure = StandardPressureAtSeaLevel * pressureFactor;

            // Convert Pascals to inches of mercury (inHg)
            const double pascalsPerInHg = 3386.389;
            return Math.Round(atmosphericPressure / pascalsPerInHg, 3);
        }                
    }
}
