using System;

namespace IAD_zad2.Utilities.Generators
{
    public static class RandomNumberProvider
    {
        private static readonly Random Rand = new Random();

        public static double GetRandomNumber(double min, double max)
        {
            return (max - min) * Rand.NextDouble() + min;
        }
    }
}