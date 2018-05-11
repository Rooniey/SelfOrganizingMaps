using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.Data.Norm
{
    public interface INormalizer
    {
        void Normalize(List<Vector<double>> data);
    }
}