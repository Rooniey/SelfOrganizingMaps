using System.Collections.Generic;
using System.Linq;
using IAD_zad2.Model.Parameters;
using IAD_zad2.Utilities.Distance;
using IAD_zad2.Utilities.Generators;
using IAD_zad2.Utilities.Observer;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Model
{
    public abstract class NeuralNetwork
    {
        public List<Neuron> Neurons { get; set; }
        public int Dimensions { get; set; }
        public ITrainingObserver Observer { get; set; }
        
        public IDistanceCalculator DistanceCalculator { get; }


        protected NeuralNetwork(int numberOfNeurons,
                             INeuronInitializer neuronInit,
                             IDistanceCalculator distanceCalculator)
        {
            DistanceCalculator = distanceCalculator;
            Neurons = new List<Neuron>();

            Dimensions = neuronInit.Dimensions;
            for (int i = 0; i < numberOfNeurons; i++)
            {
                var neuron = new Neuron();
                neuronInit.InitializeNeuron(neuron);
                Neurons.Add(neuron);
            }
        }

        public abstract void Train(TrainingParameters parameters);
        public int GetWinnerIndex(Vector<double> input)
        {
            return Neurons.IndexOf(GetWinner(input));
        }

        public Neuron GetWinner(Vector<double> input)
        {
            return Neurons
                .Select((n, i) =>
                    new {neuron = n, value = DistanceCalculator.CalculateDistance(n.CurrentWeights, input)})
                .OrderBy(item => item.value)
                .First().neuron;
        }
    }
}
