using System;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.Distance
{
    public class EuclideanDistance : IDistanceCalculator
    {
        public double CalculateDistance(Vector<double> testInput, Vector<double> current)
        {
            return Math.Sqrt( current.Subtract(testInput).Map(val => val * val).Sum() );
        }
    }
}
