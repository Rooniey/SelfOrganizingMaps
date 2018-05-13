using IAD_zad2.Exceptions;
using IAD_zad2.Utilities.Distance;
using IAD_zad2.Utilities.ExtensionMethods;
using IAD_zad2.Utilities.Generators;
using IAD_zad2.Utilities.Observer;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using System.Linq;
using IAD_zad2.Model.Parameters;

namespace IAD_zad2.Model
{
    public class SelfOrganizingMap : NeuralNetwork
    {

        public SelfOrganizingMap(int numberOfNeurons,
            INeuronInitializer neuronInit,
            IDistanceCalculator distance) : base(numberOfNeurons, neuronInit, distance)
        {
            
        }


        public override void Train(TrainingParameters parameters)
        {
            SomTrainingParameters p = (SomTrainingParameters) parameters;
            try
            {
                parameters.Validate();
            }
            catch(TrainingParametersException e)
            {
                throw new SomLogicalException($"Improper training parameters. {e.Message}");
            }
            
            var trainingData = p.TrainingData;
            var iterations = p.NumberOfIterations;
            var neighborhoodFunction = p.NeighbourhoodFunction;
            var learningRate = p.LearningRate;
            var tiredness = p.TirednessMechanism;


            if (trainingData.First().Count != Dimensions)
                throw new SomLogicalException("Training data points dimensions doesn't match som dimensions.");

            tiredness?.Initialize(Neurons);
            Observer?.SaveState(Neurons);

            int numberOfShuffles = (iterations / trainingData.Count) + 1;
            for (int j = 0; j < numberOfShuffles; j++)
            {
                var shuffled = trainingData.Shuffle();
                for (int i = 0; i < shuffled.Count; i++)
                {
                    if (j * shuffled.Count + i == iterations) return;
                    Dictionary<Neuron, double> neuronsDistances = new Dictionary<Neuron, double>();

                    for (int k = 0; k < Neurons.Count; k++)
                    {
                        //store neurons with their distances
                        neuronsDistances.Add(Neurons[k],
                            DistanceCalculator.CalculateDistance(shuffled[i], Neurons[k].CurrentWeights));
                    }

                    var potentialWinners = tiredness == null ? Neurons : tiredness.SelectPotentialWinners(Neurons);

                    Neuron winner = neuronsDistances.Where(pair => potentialWinners.Contains(pair.Key))
                        .OrderBy(pair => pair.Value).First().Key;

                    tiredness?.Update(winner, Neurons);

                    var neuronsNeighbourhoodCoefficients =
                        neighborhoodFunction.CalculateNeighborhoodValues(winner, neuronsDistances, DistanceCalculator,
                            j * trainingData.Count + i);

                    foreach (var neuron in Neurons)
                    {
                        var diffVec = shuffled[i].Subtract(neuron.CurrentWeights);

                        var deltaWeight = diffVec.Multiply(learningRate.GetValue(j * trainingData.Count + i) *
                                                           neuronsNeighbourhoodCoefficients[neuron]);
                        neuron.CurrentWeights = neuron.CurrentWeights + deltaWeight;
                    }

                    Observer?.SaveState(Neurons);
                }
            }
        }
    }
}