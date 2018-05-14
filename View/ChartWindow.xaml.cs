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

        public ChartWindow(Series stat, List<Series> animationSeries)
        {
            InitializeComponent();
            PlotAnimation = new PlotAnimation(Plot);
            Stat = stat;
            Moving = animationSeries;
            MultipleSerieses = false;
        }

        public ChartWindow(List<List<Series>> animationSeries)
        {
            InitializeComponent();
            PlotAnimation = new PlotAnimation(Plot);
            Plot.IsLegendVisible = false;
            AnimationSeries = animationSeries;
            MultipleSerieses = true;
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


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs eventArgs)
        {
            try
            {
                PlotAnimation.End();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Plot.Title = NewTitleTextBox.Text;
        }
    }
}