using IAD_zad2.Utilities.Distance;
using System.Collections.Generic;
using IAD_zad2.Model;

namespace IAD_zad2.Utilities.NeighbourhoodFunction
{
    public interface INeighborhoodFunction
    {
        Dictionary<Neuron, double> CalculateNeighborhoodValues(Neuron winner,
                                                               Dictionary<Neuron, double> neuronDistances,
                                                               IDistanceCalculator distanceCalculator,
                                                               int k);
    }
}