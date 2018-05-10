using System;
using System.Collections.Generic;
using System.Linq;
using IAD_zad2.Model;
using IAD_zad2.Utilities.Distance;
using IAD_zad2.Utilities.ParametersFunctions;

namespace IAD_zad2.Utilities.NeighbourhoodFunction
{
    public class KohonenRadialNeighbourhood: INeighborhoodFunction
    {
        private IDecliner _radius;

        public KohonenRadialNeighbourhood(IDecliner radius)
        {
            _radius = radius;
        }

        public Dictionary<Neuron, double> CalculateNeighborhoodValues(Neuron winner,
                                                                      Dictionary<Neuron, double> neuronDistances,
                                                                      IDistanceCalculator distanceCalculator,
                                                                      int k)
        {

            Dictionary<Neuron, double> neuronNeighborhoodValues = new Dictionary<Neuron, double>();

            foreach (var neuronDistancePair in neuronDistances)
            {
                var neighborhoodValue =
                    distanceCalculator.CalculateDistance(winner.CurrentWeights, neuronDistancePair.Key.CurrentWeights);

                    neuronNeighborhoodValues.Add(neuronDistancePair.Key, neighborhoodValue < _radius.GetValue(k) ? 1 : 0);
            }
            return neuronNeighborhoodValues;
        }
    }
}
