using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System.Threading;
using IAD_zad2.Model;

namespace IAD_zad2.Utilities.Observer
{
    public class StandardTrainingObserver : ITrainingObserver
    {
        public List<List<Vector<double>>> HistoryOfNeurons { get; } = new List<List<Vector<double>>>();

        public void SaveState(List<Neuron> neurons)
        {
            List<Vector<double>> currentState = new List<Vector<double>>();
            foreach (var neuron in neurons)
            {
                currentState.Add(Vector<double>.Build.DenseOfVector(neuron.CurrentWeights));
            }
            HistoryOfNeurons.Add(currentState);
        }
    }
}