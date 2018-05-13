using System.Collections.Generic;
using IAD_zad2.Model;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.Observer
{
    public interface ITrainingObserver
    {
         List<List<Vector<double>>> HistoryOfNeurons { get; }
        void SaveState(List<Neuron> neuron);
    }
}