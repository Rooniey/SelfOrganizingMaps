using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2
{
    public class Neuron
    {
        public Vector<double> CurrentWeights { get; set; }
        public int NumberOfDimensions { get; set; }

        public Neuron()
        {
        }

        public Neuron(Neuron neuron)
        {
            CurrentWeights = Vector<double>.Build.DenseOfVector(neuron.CurrentWeights);
            NumberOfDimensions = neuron.NumberOfDimensions;
        }
    }
}