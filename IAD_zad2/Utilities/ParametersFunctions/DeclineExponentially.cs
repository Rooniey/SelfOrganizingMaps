using System;

namespace IAD_zad2.Utilities.ParametersFunctions
{
    public class DeclineExponentially : IDecliner
    {
        public int Kmax { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }

        public DeclineExponentially(int kmax, double minValue, double maxValue)
        {
            Kmax = kmax;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public double GetValue(int k)
        {
            return MaxValue * Math.Pow(MinValue / MaxValue, (k * 1.0) / Kmax);
        }
    }
}