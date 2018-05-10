using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Wpf;

namespace View
{
    /// <summary>
    /// Interaction logic for ChartWindow.xaml
    /// </summary>
    public partial class ChartWindow : Window
    {
        public List<Series> AllSeries { get; set; }
        public Series CurrentSeries { get; set; }

        public Thread AnimationThread { get; set; }

        public ChartWindow(List<Series> series)
        {
            InitializeComponent();
            AllSeries = series;
        }

        private void RunAnimationButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnimationThread == null || !AnimationThread.IsAlive)
            {
                AnimationThread = new Thread(() =>
                {
                    


                    int i = 2;
                    Plot.Dispatcher.Invoke((Action)(() =>
                    {



                        Plot.Series.Add(AllSeries[0]);
                        Plot.Series.Add(AllSeries[1]);

                    }));
                   
                    while (i < AllSeries.Count)
                    {
                        Thread.Sleep(1000);
                        Plot.Dispatcher.Invoke((Action) (() =>
                        {



                            Plot.Series[1].ItemsSource = AllSeries[i].ItemsSource;
                            
                            Plot.InvalidatePlot(true);

                        }));
                        CurrentSeries = AllSeries[i];

                        i++;
                       

                    }
                });

                AnimationThread.Start();
            }



        }

    }
}
