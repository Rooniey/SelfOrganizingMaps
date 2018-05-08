using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAD_zad2.Utilities;

namespace IAD_zad2
{
    public class KohonenRadialNeighborhood: INeighborhoodFunction 
    {
        public double InitialWidth { get; set; }
        public double EndWidth { get; set; }
        public int Kmax { get; set; }

        public KohonenRadialNeighborhood(double width, double minWidth)
        {
            InitialWidth = width;
            EndWidth = minWidth;
        }

        public double GetWidth(int k)
        {
            return InitialWidth * Math.Pow(EndWidth / InitialWidth, (k * 1.0 )/ Kmax);
        }

        public void Initialize(int kmax)
        {
            Kmax = kmax;
        }

        public Dictionary<Neuron, double> CalculateNeighborhoodValues(Dictionary<Neuron, double> neuronDistances, IDistanceCalculator distanceCalculator, int k)
        {
            Neuron winner = neuronDistances.OrderBy(pair => pair.Value).First().Key;

            Dictionary<Neuron, double> neuronNeighborhoodValues = new Dictionary<Neuron, double>();

            foreach (var neuronDistancePair in neuronDistances)
            {
                var neighborhoodValue =
                    distanceCalculator.CalculateDistance(winner.CurrentWeights, neuronDistancePair.Key.CurrentWeights);

                neuronNeighborhoodValues.Add(neuronDistancePair.Key, neighborhoodValue < GetWidth(k) ? 1 : 0 );
            }

            return neuronNeighborhoodValues;
        }
    }
}
