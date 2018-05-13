using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OxyPlot.Wpf;

namespace View.Utility
{
    public class PlotAnimation
    {
        public Thread AnimationThread { get; set; }
        public Plot Plot { get; set; }
        public int Miliseconds { get; set; }
        public int CurrentIteration { get; set; }
        public event EventHandler AnimationNextFrame;

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
                Plot.Series.Add(stat);
                Plot.InvalidatePlot();
                AnimationThread = new Thread(() =>
                {
                    IsRunning = true;
                    CurrentIteration = 0;
                    while (CurrentIteration < seriesList.Count)
                    {
                        if (IsRunning)
                        {
                            Thread.Sleep(Miliseconds);
                            Plot.Dispatcher.Invoke((Action) (() =>
                            {
                                if (CurrentIteration > 0) Plot.Series.RemoveAt(1);
                                Plot.Series.Add(seriesList[CurrentIteration]);
                                Plot.InvalidatePlot();
                                OnAnimationNextFrame(new AnimationNextFrameEventArgs(CurrentIteration));
                            }));
                            CurrentIteration++;
                        }
                    }
                });
                AnimationThread.Start();
            }
        }

        protected virtual void OnAnimationNextFrame(EventArgs e)
        {
            EventHandler handler = AnimationNextFrame;
            handler?.Invoke(this, e);
        }

        public void RunMultipleSeries(int miliseconds, List<List<Series>> series)
        {
            if (AnimationThread == null || !AnimationThread.IsAlive)
            {
                AnimationThread = new Thread(() =>
                {
                    IsRunning = true;
                    CurrentIteration = 0;
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

                                Plot.InvalidatePlot();
                            }));
                            CurrentIteration++;
                        }
                    }
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
