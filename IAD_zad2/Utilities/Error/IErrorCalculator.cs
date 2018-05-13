using System.Collections.Generic;
using IAD_zad2.Model;
using IAD_zad2.Utilities.Distance;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.Error
{
    public interface IErrorCalculator
    {
        double CalculateError(List<Neuron> neurons, List<Vector<double>> input);
    }
}
