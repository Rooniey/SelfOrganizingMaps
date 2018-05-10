using System.Collections.Generic;
using System.Linq;

namespace IAD_zad2.Utilities.ParametersFunctions
{
    public class PotentialTiredness : ITirednessMechanism
    {
        public double StartingPotential { get; set; }
        public double MinimalPotential { get; set; }
        public double RegenerationConstant { get; set; }

        private Dictionary<Neuron, double> NeuronsPotential { get; set; } = new Dictionary<Neuron, double>();

        public PotentialTiredness(double startingPotential, double minimalPotential, double regenerationConstant)
        {
            StartingPotential = startingPotential;
            MinimalPotential = minimalPotential;
            RegenerationConstant = regenerationConstant;
        }

        public void Initialize(List<Neuron> neurons)
        {
            foreach (var neuron in neurons)
            {
                NeuronsPotential.Add(neuron, StartingPotential);
            }
        }

        public List<Neuron> SelectPotentialWinners(List<Neuron> neurons)
        {
            var potentialWinners = neurons.Where(n => NeuronsPotential[n] > MinimalPotential).ToList();
            if (potentialWinners.Count == 0)
            {
                foreach (var neuron in neurons)
                {
                    NeuronsPotential[neuron] = StartingPotential;
                }

                return neurons;
            }

            return potentialWinners;
        }

        public void Update(Neuron winner, List<Neuron> neurons)
        {
            foreach (var neuron in neurons)
            {
                if (neuron == winner)
                {
                    NeuronsPotential[neuron] -= MinimalPotential;
                }
                else
                {
                    NeuronsPotential[neuron] += RegenerationConstant;
                }
            }
        }
    }
}
