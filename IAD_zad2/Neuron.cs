using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2
{
    public class Neuron
    {
        public Vector<double> CurrentWeights { get; set; }
        public List<Vector<double>> HistoryOfWeights { get; set; } = new List<Vector<double>>();

        private static Random rand = new Random();

        public Neuron(int numberOfDimensions)
        {
            List<double> initialWeights = new List<double>();
            for (int i = 0; i < numberOfDimensions; i++)
            {
                initialWeights.Add(2*rand.NextDouble() - 1);
            }

            CurrentWeights = Vector<double>.Build.DenseOfEnumerable(initialWeights);
            HistoryOfWeights.Add(Vector<double>.Build.DenseOfVector(CurrentWeights));
        }
        public void SaveWeights()
        {
            HistoryOfWeights.Add(Vector<double>.Build.DenseOfVector(CurrentWeights));
        }
    }
}
