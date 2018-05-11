using System;
using System.Collections.Generic;
using System.Linq;
using IAD_zad2.Utilities.Distance;
using IAD_zad2.Utilities.ParametersFunctions;

namespace IAD_zad2.Utilities.NeighbourhoodFunction
{
    public class KohonenGaussianNeighbourhood : INeighborhoodFunction
    {
        private IDecliner _radius;

        public KohonenGaussianNeighbourhood(IDecliner radius)
        {
            _radius = radius;
        }

        public Dictionary<Neuron, double> CalculateNeighborhoodValues(Neuron winner,
                                                                      Dictionary<Neuron, double> neuronDistances,
                                                                      IDistanceCalculator distanceCalculator,
                                                                      int k)
        {

            Dictionary<Neuron, double> neuronNeighborhoodValues = new Dictionary<Neuron, double>();

            var currentRadius = Math.Abs(_radius.GetValue(k)) < 0.00001 ? 0.000001 : _radius.GetValue(k);
            foreach (var neuronDistancePair in neuronDistances)
            {
                
                var vecDiff =
                    distanceCalculator.CalculateDistance(winner.CurrentWeights, neuronDistancePair.Key.CurrentWeights);
                var exponent = (-1.0) * ((vecDiff * vecDiff) / (2 * currentRadius * currentRadius));
                var neighborhoodValue = Math.Exp(exponent);

                neuronNeighborhoodValues.Add(neuronDistancePair.Key, neighborhoodValue);
            }
            return neuronNeighborhoodValues;
        }
    }
}
