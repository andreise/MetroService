using System;

namespace MetroModel
{
    /// <summary>
    /// Metro Factory
    /// </summary>
    public static class MetroFactory
    {

        /// <summary>
        /// Creates a new Metro instance
        /// </summary>
        /// <param name="args">Input parameters encoded in the string array</param>
        /// <returns>Returns a new Metro instance</returns>
        /// <exception cref="ArgumentNullException">Throws if the args is null</exception>
        /// <exception cref="ArgumentException">Throws if any arg value is undefined or unknown</exception>
        public static IMetro CreateMetro(string[] args)
        {
            return Metro.Create(args);
        }
    }
}
