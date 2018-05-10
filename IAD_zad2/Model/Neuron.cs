using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2
{
    public class Neuron
    {
        public Vector<double> CurrentWeights { get; set; }
        public List<Vector<double>> HistoryOfWeights { get; set; } = new List<Vector<double>>();
        public int NumberOfDimensions { get; set; }

        public Neuron()
        { 
        }
        public void SaveWeights()
        {
            HistoryOfWeights.Add(Vector<double>.Build.DenseOfVector(CurrentWeights));
        }

        public Neuron(Neuron neuron)
        {
            CurrentWeights = Vector<double>.Build.DenseOfVector(neuron.CurrentWeights);
            NumberOfDimensions = neuron.NumberOfDimensions;
        }
    }
}
