namespace Services.Interfaces
{
    public interface IPsychrometricCalculator
    {
        /// <summary>
        /// Function to calculate GPP given dry bulb and relative humidity.
        /// </summary>
        /// <param name="dryBulbTemp">Dry bulb temperature (F).</param>
        /// <param name="relHumidity">Relative humidity (%).</param>
        /// <param name="elevation">Elevation in meters).</param>
        /// <returns>The calculated GPP.</returns>
        double CalculateGpp(int dryBulbTemp, int relHumidity, int elevation);

        /// <summary>
        /// Function to calculate GPP given dry bulb and relative humidity.
        /// </summary>
        /// <param name="dryBulbTemp">Dry bulb temperature (F).</param>
        /// <param name="relHumidity">Relative humidity (%).</param>
        /// <param name="elevation">Elevation in meters).</param>
        /// <returns>The calculated GPP.</returns>
        double CalculateGpp(double dryBulbTemp, double relHumidity, double elevation);
    }
}
