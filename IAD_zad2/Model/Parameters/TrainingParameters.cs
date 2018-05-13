using System.Collections.Generic;
using IAD_zad2.Exceptions;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Model.Parameters
{
    public class TrainingParameters
    {
        public List<Vector<double>> TrainingData { get; set; } = new List<Vector<double>>();

        public void Validate()
        {
            if (TrainingData.Count == 0) throw new TrainingParametersException("Training data set can not be empty.");
        }

    }
}
