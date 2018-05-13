using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using View.Utility;

namespace View
{
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        public Series Stat { get; set; }
        public List<Series> Moving { get; set; }
        public List<List<Series>> AnimationSeries { get; set; }
        public PlotAnimation PlotAnimation { get; set; }
        public bool MultipleSerieses { get;  }
        public Thread ThreadUiUpdate { get; }

        public ChartWindow(Series stat, List<Series> animationSeries)
        {
            InitializeComponent();
            Stat = stat;
            PlotAnimation = new PlotAnimation(Plot);
            PlotAnimation.AnimationNextFrame += UpdateTimeline;
            ThreadUiUpdate = new Thread(() => { PlotAnimation.AnimationNextFrame += UpdateTimeline; });
            ThreadUiUpdate.Start();
            Moving = animationSeries;
            MultipleSerieses = false;
            TimeLine.SelectionStart = 0;
            TimeLine.SelectionEnd = Moving.Count;
        }

        public ChartWindow(List<List<Series>> animationSeries)
        {
            InitializeComponent();
            PlotAnimation = new PlotAnimation(Plot);
            ThreadUiUpdate = new Thread(() => { PlotAnimation.AnimationNextFrame += UpdateTimeline; });
            ThreadUiUpdate.Start();
            AnimationSeries = animationSeries;
            MultipleSerieses = true;
            TimeLine.SelectionStart = 0;
            TimeLine.SelectionEnd = AnimationSeries.Count;
        }

        public ChartWindow()
        {
            InitializeComponent();
        }

        private void RunAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!PlotAnimation.IsRunning)
            {
                try
                {
                    int speed = Int32.Parse(SpeedInMiliseconds.Text);
                    if (MultipleSerieses)
                    {
                        PlotAnimation.RunMultipleSeries(speed, AnimationSeries);
                    }
                    else
                    {
                        PlotAnimation.RunMultipleAnimationsWithOneStatic(speed, Stat, Moving);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Something went wrong while starting animation. Original message: {ex.Message}");
                }
            }
        }

        private void UpdateTimeline(object sender, EventArgs e)
        {
            AnimationNextFrameEventArgs p = (AnimationNextFrameEventArgs) e;
            TimeLine.Value = p.CurrentIteration;
        }
        

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            try
            {
                PlotAnimation.End();
                ThreadUiUpdate.Abort();
            }
            catch (ThreadAbortException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if(PlotAnimation.IsRunning)
                PlotAnimation.Pause();
            else 
                PlotAnimation.Resume();
        }
    }
}