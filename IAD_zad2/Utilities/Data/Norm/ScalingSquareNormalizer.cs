using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace IAD_zad2.Utilities.Data.Norm
{
    public class ScalingSquareNormalizer : INormalizer
    {
        public int Range { get; set; }

        public ScalingSquareNormalizer(int range)
        {
            Range = range;
        }

        public void Normalize(List<Vector<double>> data)
        {
            var xMax = data.Max(v => v.At(0));
            var xMin = data.Min(v => v.At(0));
            var yMax = data.Max(v => v.At(0));
            var yMin = data.Min(v => v.At(0));

            foreach (var vec in data)
            {
                vec[0] = ((vec[0] - xMin) / (xMax - xMin)) * (2 * Range) - Range;
                vec[1] = ((vec[1] - yMin) / (yMax - yMin)) * (2 * Range) - Range;
            }
        }
    }
}