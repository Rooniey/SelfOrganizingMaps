using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace IAD_zad2.Utilities
{
    public interface IDistanceCalculator
    {
        double CalculateDistance(Vector<double> testInput, Vector<double> current);
    }
}
