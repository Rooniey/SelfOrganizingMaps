using System.Collections.Generic;
using System.Linq;
using IAD_zad2.Utilities.Distance;
using IAD_zad2.Utilities.ExtensionMethods;
using IAD_zad2.Utilities.Generators;
using IAD_zad2.Utilities.NeighbourhoodFunction;
using IAD_zad2.Utilities.ParametersFunctions;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Model
{
    public class SelfOrganizingMap
    {
        public List<Neuron> Neurons { get; set; } = new List<Neuron>();
        private IDistanceCalculator _distCal;        

        public SelfOrganizingMap(int numberOfNeurons, 
                                 INeuronInitializer neuronInit,
                                 IDistanceCalculator distance)
        {
            _distCal = distance;
            for (int i = 0; i < numberOfNeurons; i++)
            {
                var neuron = new Neuron();
                neuronInit.InitializeNeuron(neuron);
                Neurons.Add(neuron);
            }
        }

        public void Train(List<Vector<double> > trainingData, 
                          int epochs,
                          INeighborhoodFunction neighborhoodFunction,   
                          IDecliner learningRate,
                          ITirednessMechanism tiredness = null
            )
        {
            tiredness?.Initialize(Neurons);

            var shuffled = trainingData.Shuffle();

            for (int j = 0; j < epochs; j++)
            {
                for (int i = 0; i < shuffled.Count; i++)
                {
                    Dictionary<Neuron, double> neuronsDistances = new Dictionary<Neuron, double>();

                    for (int k = 0; k < Neurons.Count; k++)
                    {
                        //store neurons with their distances
                        neuronsDistances.Add(Neurons[k], _distCal.CalculateDistance(shuffled[i], Neurons[k].CurrentWeights));
                    }

                    var potentialWinners = tiredness == null ? Neurons : tiredness.SelectPotentialWinners(Neurons);

                    Neuron winner = neuronsDistances.Where(pair => potentialWinners.Contains(pair.Key))
                          .OrderBy(pair => pair.Value).First().Key;

                    tiredness?.Update(winner,Neurons);

                    var neuronsNeighbourhoodCoefficients = neighborhoodFunction.CalculateNeighborhoodValues(winner, neuronsDistances, _distCal, i);

                    foreach (var neuron in Neurons)
                    {
                        var diffVec = shuffled[i].Subtract(neuron.CurrentWeights);

                        var deltaWeight = diffVec.Multiply( learningRate.GetValue(i) * neuronsNeighbourhoodCoefficients[neuron]);
                        neuron.CurrentWeights += deltaWeight;
                    }
                }

                SaveState();
            }
        }

        private void SaveState()
        {
            foreach (var neuron in Neurons)
            {
                neuron.SaveWeights();
            }
        }
    }
}
