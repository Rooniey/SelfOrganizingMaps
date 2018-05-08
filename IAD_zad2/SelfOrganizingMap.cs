using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAD_zad2.Utilities;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace IAD_zad2
{
    public class SelfOrganizingMap
    {
        public List<Neuron> Neurons { get; set; } = new List<Neuron>();
        private IDistanceCalculator _distCal;
        

        public SelfOrganizingMap(int numberOfNeurons, int numberOfDimensions, IDistanceCalculator distance)
        {
            _distCal = distance;
            for (int i = 0; i < numberOfNeurons; i++)
            {
                Neurons.Add(new Neuron(numberOfDimensions));
            }
        }

        public void Train(List<Vector<double> > trainingData, int epochs, INeighborhoodFunction neighborhoodFunction, double learningRate, double minLearningRate)
        {
            neighborhoodFunction.Initialize(trainingData.Count - 1);
            var shuffled = trainingData.OrderBy(a => Guid.NewGuid()).ToList();

            for (int j = 0; j < epochs; j++)
            {
               
                for (int i = 0; i < shuffled.Count; i++)
                {
                    Dictionary<Neuron, double> neuronsDistances = new Dictionary<Neuron, double>();
                    for (int k = 0; k < Neurons.Count; k++)
                    {
                        neuronsDistances.Add(Neurons[k], _distCal.CalculateDistance(shuffled[i], Neurons[k].CurrentWeights));//store neurons with their distances
                    }

                    var neuronsNeighbourhoodCoefficients = neighborhoodFunction.CalculateNeighborhoodValues(neuronsDistances, _distCal, i);
                    foreach (var neuron in Neurons)
                    {
                        var diffVec = shuffled[i].Subtract(neuron.CurrentWeights);
                        var currLearningRate =
                            GetLearningRate(learningRate, minLearningRate, i, trainingData.Count - 1);
                        var deltaWeight = diffVec.Multiply( currLearningRate * neuronsNeighbourhoodCoefficients[neuron]);
                        neuron.CurrentWeights += deltaWeight;
                    }
                    

                }
                foreach (var neuron in Neurons)
                {
                    neuron.SaveWeights();
                }
            }
            
        }

        private double GetLearningRate(double learningRate, double minLearningRate, int k, int kmax)
        {
            return learningRate * Math.Pow(minLearningRate / learningRate, (k * 1.0) / kmax);
        }
    }
}
