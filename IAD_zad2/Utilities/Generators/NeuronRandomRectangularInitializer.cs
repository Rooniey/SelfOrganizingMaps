using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.Generators
{
    public class NeuronRandomRectangularInitializer : INeuronInitializer
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public int Dimensions { get; set; }

        private static Random _rand = new Random();

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
                initialWeights.Add( (Max-Min) * _rand.NextDouble() + Min);
            }

            neuron.CurrentWeights = Vector<double>.Build.DenseOfEnumerable(initialWeights);
            neuron.HistoryOfWeights.Clear();
            neuron.HistoryOfWeights.Add(Vector<double>.Build.DenseOfVector(neuron.CurrentWeights));
 
        }
    }
}
