﻿using MathNet.Numerics.LinearAlgebra;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Windows.Media;
using IAD_zad2.Utilities.Generators;
using OxyPlot;

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
                    MarkerSize = 5,
                    MarkerType = MarkerType.Circle,
                    MarkerFill = Color.FromRgb(0, 0,0)
                });
               
            }

            return scatterSeries;
        }


        public static ScatterPoint ConvertVectorToScatterPoint(Vector<double> data, bool respect = false)
        {
            var x = data.At(0);
            var y = data.At(1);
            return new ScatterPoint(x,y, respect ? 6 : 3);
        }

    }
}