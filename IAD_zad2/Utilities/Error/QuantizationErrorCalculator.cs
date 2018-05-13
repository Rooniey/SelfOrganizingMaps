using System.Collections.Generic;
using System.Linq;
using IAD_zad2.Model;
using IAD_zad2.Utilities.Distance;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.Error
{
    public class QuantizationErrorCalculator : IErrorCalculator
    {
        public IDistanceCalculator DistanceCalculator { get; set; }

        public double CalculateError(List<Neuron> neurons, List<Vector<double>> inputs)
        {
            double sum = 0;
            foreach (var input in inputs)
            {
                Neuron winnner = neurons.Select((n, i) =>
                        new {neuron = n, value = DistanceCalculator.CalculateDistance(n.CurrentWeights, input)})
                    .OrderBy(item => item.value)
                    .First().neuron;

                sum += DistanceCalculator.CalculateDistance(winnner.CurrentWeights, input);
            }

            return sum / inputs.Count;

        }
    }
}
