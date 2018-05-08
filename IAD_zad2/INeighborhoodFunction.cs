using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAD_zad2.Utilities;

namespace IAD_zad2
{
    public interface INeighborhoodFunction
    {
        void Initialize(int kmax);
        Dictionary<Neuron, double> CalculateNeighborhoodValues(Dictionary<Neuron, double> neuronDistances, IDistanceCalculator distanceCalculator, int k);
    }
}
