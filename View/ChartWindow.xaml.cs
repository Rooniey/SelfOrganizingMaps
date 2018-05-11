using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace View
{
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        public List<Series> AllSeries { get; set; }
        public Series CurrentSeries { get; set; }
        public int Kmax { get; set; }

        public Thread AnimationThread { get; set; }

        public ChartWindow(List<Series> series, int kmax)
        {
            InitializeComponent();
            Kmax = kmax;
            AllSeries = series;
        }

        private void RunAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnimationThread == null)
            {
                AnimationThread = new Thread(() =>
                {
                    int i = 2;
                    Plot.Dispatcher.Invoke((Action)(() =>
                    {
                        Plot.Series.Add(AllSeries[0]);
                        Plot.Series.Add(AllSeries[1]);
                        Plot.Title = $"epoch = 1; k = {i - 1}";
                    }));

                    Thread.Sleep(300);
                    while (i < AllSeries.Count)
                    {
                        Plot.Dispatcher.Invoke((Action)(() =>
                       {
                           Plot.Series[1].ItemsSource = AllSeries[i].ItemsSource;
                           Plot.InvalidatePlot(true);
                           Plot.Title = $"epoch = {(i - 1) / Kmax} k = {i - 1}";
                       }));
                        CurrentSeries = AllSeries[i];
                        i++;
                    }
                });
                AnimationThread.Start();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            try
            {
                AnimationThread?.Abort();
            }
            catch (ThreadAbortException e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}