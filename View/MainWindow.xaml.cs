using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IAD_zad2;
using IAD_zad2.Model;
using IAD_zad2.Utilities;
using IAD_zad2.Utilities.Distance;
using IAD_zad2.Utilities.ExtensionMethods;
using IAD_zad2.Utilities.Generators;
using IAD_zad2.Utilities.NeighbourhoodFunction;
using IAD_zad2.Utilities.ParametersFunctions;
using MathNet.Numerics.LinearAlgebra;
using OxyPlot.Series;
using View.ViewModels;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<OxyPlot.Wpf.Series> ser = new List<OxyPlot.Wpf.Series>();
            List<Vector<double>> training = new List<Vector<double>>();
            Random rand = new Random();

            this.numbers = new MainWindowViewModel
            {
                PointsCount = Int32.Parse(PointsCount.Text),
                NeuronCount = Int32.Parse(NeuronCount.Text),
                EpochsCount = Int32.Parse(EpochsCount.Text),
                LearningRateMAX = Double.Parse(LearningRateMAX.Text, CultureInfo.InvariantCulture),
                LearningRateMIN = Double.Parse(LearningRateMIN.Text, CultureInfo.InvariantCulture),
                KohonenNeighRadMAX = Double.Parse(KohonenNeighRadMAX.Text, CultureInfo.InvariantCulture),
                KohonenNeighRadMIN = Double.Parse(KohonenNeighRadMIN.Text, CultureInfo.InvariantCulture)
            };

            for (int i = 0; i < numbers.PointsCount; i++)
            {
//                var vec = Vector<double>.Build.Dense(2, kupa => (2 * rand.NextDouble() - 1));
                var vec = Vector<double>.Build.Dense(2, kupa => (rand.NextDouble() - 1));
                training.Add(vec);
            }

            var som = new SelfOrganizingMap(numbers.NeuronCount, new NeuronRandomRectangularInitializer(-1,1, 2), new ManhattanDistance());
            som.Train(
                training, 
                numbers.EpochsCount, 
                new KohonenGaussianNeighbourhood(new DeclineExponentially(training.Count, numbers.KohonenNeighRadMIN, numbers.KohonenNeighRadMAX)), 
                new DeclineExponentially(training.Count, numbers.LearningRateMIN, numbers.LearningRateMAX ),
                new PotentialTiredness(1, 0.8, 0.1));

            
            var scatters1 = new List<ScatterPoint>();
            foreach (var train in training)
            {

                var x1 = train.At(0);
                var y1 = train.At(1);
                scatters1.Add(new ScatterPoint(x1, y1));
            }
            scatters1.Add(new ScatterPoint(-2, -2));
            scatters1.Add(new ScatterPoint(2, 2));
            scatters1 = scatters1.Shuffle();



            ser.Add(new OxyPlot.Wpf.ScatterSeries
            {
                ItemsSource = scatters1,
                Title = $"Training points",
                MarkerSize = 2
            });

            for (int j = 0; j < numbers.EpochsCount; j++)
            {
                var scatters = new List<ScatterPoint>();
                for (int i = 0; i <  numbers.NeuronCount;i++)
                {
                    var x = som.Neurons[i].HistoryOfWeights[j].At(0);
                    var y = som.Neurons[i].HistoryOfWeights[j].At(1);
                    scatters.Add(new ScatterPoint(x, y));
                }


                ser.Add(new OxyPlot.Wpf.ScatterSeries
                {
                    ItemsSource = scatters,
                    Title = $"Neurons",
                    MarkerSize = 5
                });
            }

            ChartWindow chw = new ChartWindow(ser);
            chw.Show();
        }

        private MainWindowViewModel numbers;
    }
}
