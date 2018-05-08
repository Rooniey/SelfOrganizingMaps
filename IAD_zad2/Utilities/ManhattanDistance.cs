﻿using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities
{
    public class ManhattanDistance : IDistanceCalculator
    {
        public double CalculateDistance(Vector<double> testInput, Vector<double> current)
        {
            return current.Subtract(testInput).SumMagnitudes();
        }
    }
}
