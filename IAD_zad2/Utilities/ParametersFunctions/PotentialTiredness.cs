using System;
using System.Collections.Generic;
using System.Linq;
using IAD_zad2.Model;

namespace IAD_zad2.Utilities.ParametersFunctions
{
    public class PotentialTiredness : ITirednessMechanism
    {
        public double StartingPotential { get; set; }
        public double MinimalPotential { get; set; }
        public double RegenerationConstant { get; set; }

        public int UpperLimit { get; set; }
        public int CurrentIteration { get; set; }

        private bool _isOn;
        public bool IsOn { get => _isOn; }

        private Dictionary<Neuron, double> NeuronsPotential { get; } = new Dictionary<Neuron, double>();

        public PotentialTiredness(double startingPotential, double minimalPotential, double regenerationConstant, int upperLimit = Int32.MaxValue)
        {
            StartingPotential = startingPotential;
            MinimalPotential = minimalPotential;
            RegenerationConstant = regenerationConstant;
            UpperLimit = upperLimit;
        }

        public void Initialize(List<Neuron> neurons)
        {
            _isOn = true;
            CurrentIteration = 0;
            foreach (var neuron in neurons)
            {
                NeuronsPotential.Add(neuron, StartingPotential);
            }
        }

        public List<Neuron> SelectPotentialWinners(List<Neuron> neurons)
        {
            if (CurrentIteration > UpperLimit) Deactivate();
            if (IsOn)
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

            return neurons;
        }

        public void Update(Neuron winner, List<Neuron> neurons)
        {
            if (IsOn)
            {
                foreach (var neuron in neurons)
                {
                    if (neuron == winner)
                    {
                        var diff = NeuronsPotential[neuron] - MinimalPotential;
                        NeuronsPotential[neuron] = diff >= 0 ? diff : 0;
                    }
                    else
                    {
                        var sum = NeuronsPotential[neuron] + RegenerationConstant;
                        NeuronsPotential[neuron] = sum <= StartingPotential ? sum : StartingPotential;
                    }
                }
            }

            CurrentIteration += 1;
        }

        public void Deactivate()
        {
            _isOn = false;
        }
    }
}