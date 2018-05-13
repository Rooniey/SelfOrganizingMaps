using MathNet.Numerics.LinearAlgebra;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Windows.Media;

namespace View.Utility
{
    public static class Converter
    {
        public static List<OxyPlot.Wpf.Series> ConvertToScatterSeries(List<List<Vector<double>>> historyOfNeurons)
        {
            List<OxyPlot.Wpf.Series> scatterSeries = new List<OxyPlot.Wpf.Series>();
            for(int i = 0; i < historyOfNeurons.Count; i++)
            {
                List<ScatterPoint> it = new List<ScatterPoint>();

                foreach (var neuron in historyOfNeurons[i])
                {
                    var x = neuron.At(0);
                    var y = neuron.At(1);
                    it.Add(new ScatterPoint(x,y));
                }
                scatterSeries.Add(new OxyPlot.Wpf.ScatterSeries
                {
                    ItemsSource = it,
                    Title = $"Neurons",
                    Color = Color.FromRgb(191,91,69),
                    MarkerSize = 4
                });
               
            }

            return scatterSeries;
        }
        

        public static ScatterPoint ConvertVectorToScatterPoint(Vector<double> data)
        {
            var x = data.At(0);
            var y = data.At(1);
            return new ScatterPoint(x,y);
        }

    }
}