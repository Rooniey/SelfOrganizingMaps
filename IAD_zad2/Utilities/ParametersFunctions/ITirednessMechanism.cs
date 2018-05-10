using System.Collections.Generic;

namespace IAD_zad2.Utilities.ParametersFunctions
{
    public interface ITirednessMechanism
    {
        void Initialize(List<Neuron> neurons);

        List<Neuron> SelectPotentialWinners(List<Neuron> neurons);

        void Update(Neuron winner, List<Neuron> neurons);

    }
}
