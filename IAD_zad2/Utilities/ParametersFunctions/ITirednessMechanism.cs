using System.Collections.Generic;
using IAD_zad2.Model;

namespace IAD_zad2.Utilities.ParametersFunctions
{
    public interface ITirednessMechanism
    {
        void Initialize(List<Neuron> neurons);

        List<Neuron> SelectPotentialWinners(List<Neuron> neurons);

        void Update(Neuron winner, List<Neuron> neurons);
    }
}