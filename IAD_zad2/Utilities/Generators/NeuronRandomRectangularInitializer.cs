using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using IAD_zad2.Model;

namespace IAD_zad2.Utilities.Generators
{
    public class NeuronRandomRectangularInitializer : INeuronInitializer
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public int Dimensions { get; set; }

        public NeuronRandomRectangularInitializer(double min, double max, int dimensions)
        {
            Min = min;
            Max = max;
            Dimensions = dimensions;
        }

        public void InitializeNeuron(Neuron neuron)
        {
            neuron.NumberOfDimensions = Dimensions;
            List<double> initialWeights = new List<double>();
            for (int i = 0; i < Dimensions; i++)
            {
                initialWeights.Add(RandomNumberProvider.GetRandomNumber(Min, Max));
            }

            neuron.CurrentWeights = Vector<double>.Build.DenseOfEnumerable(initialWeights);
        }
    }
}