using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.Distance
{
    public interface IDistanceCalculator
    {
        double CalculateDistance(Vector<double> testInput, Vector<double> current);
    }
}