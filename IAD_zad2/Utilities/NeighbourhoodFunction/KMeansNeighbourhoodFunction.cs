using IAD_zad2.Utilities.Distance;
using System.Collections.Generic;

namespace IAD_zad2.Utilities.NeighbourhoodFunction
{
    public class KMeansNeighbourhoodFunction : INeighborhoodFunction
    {
        public Dictionary<Neuron, double> CalculateNeighborhoodValues(Neuron winner,
                                                                      Dictionary<Neuron, double> neuronDistances,
                                                                      IDistanceCalculator distanceCalculator,
                                                                      int k)
        {
            Dictionary<Neuron, double> neuronNeighborhoodValues = new Dictionary<Neuron, double>();

            foreach (var neuronDistancePair in neuronDistances)
            {
                if (neuronDistancePair.Key == winner) neuronNeighborhoodValues.Add(neuronDistancePair.Key, 1);
                else neuronNeighborhoodValues.Add(neuronDistancePair.Key, 0);
            }
            return neuronNeighborhoodValues;
        }
    }
}