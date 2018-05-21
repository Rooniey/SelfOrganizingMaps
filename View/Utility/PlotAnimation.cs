using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using OxyPlot;
using OxyPlot.Wpf;

namespace View.Utility
{
    public class PlotAnimation
    {
        public Thread AnimationThread { get; set; }
        public Plot Plot { get; set; }
        public int Miliseconds { get; set; }
        public int CurrentIteration { get; set; }
        public bool IsRunning { get; set; }

        public PlotAnimation(Plot plot)
        {
            Plot = plot;
        }

        public void Pause()
        {
            IsRunning = false;
        }

        public void Resume()
        {
            IsRunning = true;
        }

        public void RunMultipleAnimationsWithOneStatic(int miliseconds, Series stat,
            List<Series> seriesList)
        {
            
            if (AnimationThread == null || !AnimationThread.IsAlive)
            {
                IsRunning = true;
                CurrentIteration = 0;
                Plot.Series.Clear();
                Plot.Series.Add(stat);
                Plot.InvalidatePlot();
                AnimationThread = new Thread(() =>
                {
                    while (CurrentIteration < seriesList.Count)
                    {
                        if (IsRunning)
                        {
                            Thread.Sleep(Miliseconds);
                            Plot.Dispatcher.Invoke((Action) (() =>
                            {
                                if (CurrentIteration > 0) Plot.Series.RemoveAt(1);
                                Plot.Series.Add(seriesList[CurrentIteration]);
                                Plot.Title = $"{CurrentIteration}/{seriesList.Count}";
                                Plot.InvalidatePlot();
                            }));
                            CurrentIteration++;
                        }
                    }
                    IsRunning = false;
                });
                AnimationThread.Start();
            }
        }

        public void RunMultipleSeries(int miliseconds, List<List<Series>> series)
        {
           
            if (AnimationThread == null || !AnimationThread.IsAlive)
            {
                CurrentIteration = 0;
                IsRunning = true;
                Plot.Series.Clear();
                AnimationThread = new Thread(() =>
                {
                    while (CurrentIteration < series.Count)
                    {
                        if (IsRunning)
                        {
                            Thread.Sleep(Miliseconds);
                            Plot.Dispatcher.Invoke((Action) (() =>
                            {
                                Plot.Series.Clear();
                                foreach (var s in series[CurrentIteration])
                                {
                                    Plot.Series.Add(s);
                                }
                                Plot.Title = $"{CurrentIteration}/{series.Count}";
                                Plot.InvalidatePlot();
                            }));
                            CurrentIteration++;
                        }
                    }
                    IsRunning = false;
                });
                
                AnimationThread.Start();
            }
        }

        public void End()
        {
            AnimationThread?.Abort();
        }

    }
}
