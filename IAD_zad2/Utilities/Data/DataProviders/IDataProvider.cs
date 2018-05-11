using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.Data.DataProviders
{
    public interface IDataProvider
    {
        List<Vector<double>> GetData();
    }
}