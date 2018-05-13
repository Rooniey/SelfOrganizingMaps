using System;
using System.Collections.Generic;
using System.Linq;
using IAD_zad2.Exceptions;
using IAD_zad2.Model.Parameters;
using IAD_zad2.Utilities.Distance;
using IAD_zad2.Utilities.ExtensionMethods;
using IAD_zad2.Utilities.Generators;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Model
{
    public class KMeansNetwork : NeuralNetwork
    {
        public override void Train(TrainingParameters parameters)
        {
            KMeansTrainingParameters p = (KMeansTrainingParameters) parameters;
            try
            {
                p.Validate();
            }
            catch (TrainingParametersException e)
            {
                throw new SomLogicalException($"Improper training parameters. {e.Message}");
            }

            var trainingData = parameters.TrainingData;
            var epochs = p.Epochs;

            if (trainingData.First().Count != Dimensions)
                throw new SomLogicalException("Training data points dimensions doesn't match som dimensions.");


            Observer?.SaveState(Neurons);

            for (int j = 0; j < epochs; j++)
            {
                var shuffled = trainingData.Shuffle();
                Dictionary<Neuron, List<Vector<double>>> neuronsZones = new Dictionary<Neuron, List<Vector<double>>>();
                foreach (var neuron in Neurons)
                {
                    neuronsZones.Add(neuron, new List<Vector<double>>());
                }
                for (int i = 0; i < shuffled.Count; i++)
                {

                    Dictionary<Neuron, double> neuronsDistances = new Dictionary<Neuron, double>();
                    for (int k = 0; k < Neurons.Count; k++)
                    {
                        //store neurons with their distances
                        neuronsDistances.Add(Neurons[k],
                            DistanceCalculator.CalculateDistance(shuffled[i], Neurons[k].CurrentWeights));
                    }
                    Neuron winner = neuronsDistances.OrderBy(pair => pair.Value).First().Key;
                    neuronsZones[winner].Add(shuffled[i]);

                }


                var aggregate = Vector<double>.Build.DenseOfEnumerable(new List<double>(new double[Dimensions]) );
                foreach (var pair in neuronsZones)
                {
                    foreach (var vec in pair.Value)
                    {
                        aggregate = aggregate.Add(vec);
                    }

                    if (pair.Value.Count != 0)
                    {
                        aggregate = aggregate.Divide(pair.Value.Count);
                        pair.Key.CurrentWeights = aggregate;
                    }
                    
                }

                Observer?.SaveState(Neurons);
            }
        }

        public KMeansNetwork(int numberOfNeurons, 
                            INeuronInitializer neuronInit, 
                            IDistanceCalculator distanceCalculator) : base(numberOfNeurons, neuronInit, distanceCalculator)
        {

        }
    }
}
