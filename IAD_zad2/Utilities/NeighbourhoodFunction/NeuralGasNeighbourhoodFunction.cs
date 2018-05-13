using IAD_zad2.Utilities.Distance;
using IAD_zad2.Utilities.ParametersFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using IAD_zad2.Model;

namespace IAD_zad2.Utilities.NeighbourhoodFunction
{
    public class NeuralGasNeighbourhoodFunction : INeighborhoodFunction
    {
        private IDecliner _lambda;

        public NeuralGasNeighbourhoodFunction(IDecliner lambda)
        {
            _lambda = lambda;
        }

        public Dictionary<Neuron, double> CalculateNeighborhoodValues(Neuron winner,
                                                                      Dictionary<Neuron, double> neuronDistances,
                                                                      IDistanceCalculator distanceCalculator,
                                                                      int k)
        {
            var orderedNeurons = neuronDistances.OrderBy(pair => pair.Value)
                .Select((pair, index) => new { pair.Key, index })
                .ToDictionary(x => x.Key, x => x.index);

            Dictionary<Neuron, double> neuronNeighborhoodValues = new Dictionary<Neuron, double>();
            var currentLambda = Math.Abs(_lambda.GetValue(k)) < 0.00001 ? 0.000001 : _lambda.GetValue(k);

            foreach (var neuronPositionPair in orderedNeurons)
            {
                var exponent = ((-1.0) * neuronPositionPair.Value) / currentLambda;
                var neighborhoodValue = Math.Exp(exponent);

                neuronNeighborhoodValues.Add(neuronPositionPair.Key, neighborhoodValue);
            }
            return neuronNeighborhoodValues;
        }
    }
}