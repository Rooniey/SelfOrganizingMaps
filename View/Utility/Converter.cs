using MathNet.Numerics.LinearAlgebra;
using OxyPlot.Series;
using System.Collections.Generic;

namespace View.Utility
{
    public static class Converter
    {
        public static List<OxyPlot.Wpf.Series> ConvertToScatterSeries(List<List<Vector<double>>> historyOfNeurons)
        {
            List<OxyPlot.Wpf.Series> scatterSeries = new List<OxyPlot.Wpf.Series>();
            while (historyOfNeurons.Count != 0)
            {
                List<ScatterPoint> it = new List<ScatterPoint>();

                foreach (var neuron in historyOfNeurons[0])
                {
                    var x = neuron.At(0);
                    var y = neuron.At(1);
                    it.Add(new ScatterPoint(x, y));
                }
                scatterSeries.Add(new OxyPlot.Wpf.ScatterSeries
                {
                    ItemsSource = it,
                    Title = $"Neurons",
                    MarkerSize = 4
                });
                historyOfNeurons.RemoveAt(0);
            }

            return scatterSeries;
        }
    }
}