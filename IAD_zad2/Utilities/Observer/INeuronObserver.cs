using System.Collections.Generic;

namespace IAD_zad2.Utilities.Observer
{
    public interface ITrainingObserver
    {
        void SaveState(List<Neuron> neuron);
    }
}