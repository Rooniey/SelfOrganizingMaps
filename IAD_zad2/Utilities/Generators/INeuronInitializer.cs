using IAD_zad2.Model;

namespace IAD_zad2.Utilities.Generators
{
    public interface INeuronInitializer
    {
        int Dimensions { get; set; }

        void InitializeNeuron(Neuron neuron);
    }
}