using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace IAD_zad2.Utilities.Data
{
    public interface IDataProvider
    {
        List<Vector<double>> GetData();
    }
}
