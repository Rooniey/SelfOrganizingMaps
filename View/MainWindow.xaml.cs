using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using IAD_zad2.Exceptions;
using IAD_zad2.Model;
using IAD_zad2.Utilities.Data;
using IAD_zad2.Utilities.Distance;
using IAD_zad2.Utilities.Generators;
using IAD_zad2.Utilities.NeighbourhoodFunction;
using IAD_zad2.Utilities.ParametersFunctions;
using MathNet.Numerics.LinearAlgebra;
using OxyPlot.Series;


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

        public SelfOrganizingMap Som { get; set; }

        public List<Vector<double>> TrainingData { get; set; }

        public string ManhattanChoice { get; set; } = "Manhattan";
        public string EuclideanChoice { get; set; } = "Euclidean";

        public string KohonenGaussianChoice { get; set; } = "Kohonen gaussian";
        public string KohonenBubbleChoice { get; set; } = "Kohonen bubble";
        public string NeuralGasChoice { get; set; } = "Neural gas";

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //            List<OxyPlot.Wpf.Series> ser = new List<OxyPlot.Wpf.Series>();
            //            List<Vector<double>> training = new List<Vector<double>>();
            //            Random rand = new Random();
            //
            //            this.numbers = new MainWindowViewModel
            //            {
            //                PointsCount = Int32.Parse(PointsCount.Text),
            //                NeuronCount = Int32.Parse(NeuronCount.Text),
            //                EpochsCount = Int32.Parse(EpochsCount.Text),
            //                LearningRateMAX = Double.Parse(LearningRateMAX.Text, CultureInfo.InvariantCulture),
            //                LearningRateMIN = Double.Parse(LearningRateMIN.Text, CultureInfo.InvariantCulture),
            //                KohonenNeighRadMAX = Double.Parse(KohonenNeighRadMAX.Text, CultureInfo.InvariantCulture),
            //                KohonenNeighRadMIN = Double.Parse(KohonenNeighRadMIN.Text, CultureInfo.InvariantCulture)
            //            };
            //
            //            for (int i = 0; i < numbers.PointsCount; i++)
            //            {
            ////                var vec = Vector<double>.Build.Dense(2, kupa => (2 * rand.NextDouble() - 1));
            //                var vec = Vector<double>.Build.Dense(2, kupa => (rand.NextDouble() - 1));
            //                training.Add(vec);
            //            }
            //
            //            var som = new SelfOrganizingMap(numbers.NeuronCount, new NeuronRandomRectangularInitializer(-1,1, 2), new ManhattanDistance());
            //            som.Train(
            //                training, 
            //                numbers.EpochsCount, 
            //                new KohonenGaussianNeighbourhood(new DeclineExponentially(training.Count, numbers.KohonenNeighRadMIN, numbers.KohonenNeighRadMAX)), 
            //                new DeclineExponentially(training.Count, numbers.LearningRateMIN, numbers.LearningRateMAX ),
            //                new PotentialTiredness(1, 0.8, 0.1));
            //
            //            
            //            var scatters1 = new List<ScatterPoint>();
            //            foreach (var train in training)
            //            {
            //
            //                var x1 = train.At(0);
            //                var y1 = train.At(1);
            //                scatters1.Add(new ScatterPoint(x1, y1));
            //            }
            //            scatters1.Add(new ScatterPoint(-2, -2));
            //            scatters1.Add(new ScatterPoint(2, 2));
            //            scatters1 = scatters1.Shuffle();
            //
            //
            //
            //            ser.Add(new OxyPlot.Wpf.ScatterSeries
            //            {
            //                ItemsSource = scatters1,
            //                Title = $"Training points",
            //                MarkerSize = 2
            //            });
            //
            //            for (int j = 0; j < numbers.EpochsCount; j++)
            //            {
            //                var scatters = new List<ScatterPoint>();
            //                for (int i = 0; i <  numbers.NeuronCount;i++)
            //                {
            //                    var x = som.Neurons[i].HistoryOfWeights[j].At(0);
            //                    var y = som.Neurons[i].HistoryOfWeights[j].At(1);
            //                    scatters.Add(new ScatterPoint(x, y));
            //                }
            //
            //
            //                ser.Add(new OxyPlot.Wpf.ScatterSeries
            //                {
            //                    ItemsSource = scatters,
            //                    Title = $"Neurons",
            //                    MarkerSize = 5
            //                });
            //            }
            //
            //            ChartWindow chw = new ChartWindow(ser);
            //            chw.Show();
        }

        #region Creators
        private void SomCreate_Click(object sender, RoutedEventArgs eventArgs)
        {
            try
            {
                int neuronCount = Int32.Parse(SomNeuronCount.Text);
                double minWeights = double.Parse(SomWeightsMin.Text, CultureInfo.InvariantCulture);
                double maxWeights = double.Parse(SomWeightsMax.Text, CultureInfo.InvariantCulture);
                int dimensions = Int32.Parse(SomDimensions.Text);
                string choice = SomDistance.Text;
                IDistanceCalculator dist = new EuclideanDistance();
                if (choice == ManhattanChoice)
                {
                    dist = new ManhattanDistance();
                }

                Som = new SelfOrganizingMap(neuronCount, new NeuronRandomRectangularInitializer(minWeights, maxWeights, dimensions), dist);

                SomInformation.Text =
                    $"Successfully created SOM\n NC: {neuronCount}; D: {choice}; Min.: {minWeights}; Max.: {maxWeights}";

            }
            catch (Exception e)
            {
                MessageBox.Show($"Something went wrong while parsing. Original message: {e.Message}");
            }
        }
        #endregion

        private void DataFromFileButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            IDataProvider dataProvider = new FileDataProvider();
            try
            {
                TrainingData = dataProvider.GetData();
                DataInfo.Text = $"Successfully completed loading data from file.\nData dimensions: {TrainingData.First().Count}\nData count: {TrainingData.Count}";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void TrainingAlghoritmType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && ((ComboBoxItem)(e.AddedItems[0])).Content.ToString().Contains(NeuralGasChoice))
            {
                TrainParameterLabel.Text = "Neural gas lambda";

            }
            else if (e.RemovedItems.Count > 0 && ((ComboBoxItem)(e.RemovedItems[0])).Content.ToString().Contains(NeuralGasChoice))
            {
                TrainParameterLabel.Text = "Kohonen neighborhood radius";
            }
        }

        private void StartTrainButton_Click(object sender, RoutedEventArgs eventArgs)
        {

            if (Som == null || TrainingData == null)
            {
                MessageBox.Show("You didn't create network or get data points!\n");
            }
            else
            {
                try
                {
                    int epochs = Int32.Parse(TrainingEpochs.Text);
                    double minLearningRate = double.Parse(TrainingLearningRateMin.Text, CultureInfo.InvariantCulture);
                    double maxLearningRate = double.Parse(TrainingLearningRateMax.Text, CultureInfo.InvariantCulture);
                    double minParameter = double.Parse(TrainingParameterMin.Text, CultureInfo.InvariantCulture);
                    double maxParameter = double.Parse(TrainingParameterMax.Text, CultureInfo.InvariantCulture);
                    IDecliner decPara = new DeclineExponentially(TrainingData.Count, minParameter, maxParameter);
                    IDecliner learnPara = new DeclineExponentially(TrainingData.Count, minLearningRate, maxLearningRate);
                    string choice = TrainingAlghoritmType.Text;
                    INeighborhoodFunction fun = new KohonenGaussianNeighbourhood(decPara);
                    if (choice == NeuralGasChoice)
                    {
                        fun = new NeuralGasNeighbourhoodFunction(decPara);
                    }
                    else if (choice == KohonenBubbleChoice)
                    {
                        fun = new KohonenRadialNeighbourhood(decPara);
                    }
                    Som.Train(TrainingData, epochs, fun, learnPara);


                    if (Som.Dimensions == 2)
                    {
                        List<OxyPlot.Wpf.Series> series = new List<OxyPlot.Wpf.Series>();

                        var scattersData = new List<ScatterPoint>();
                        foreach (var train in TrainingData)
                        {
                            var x = train.At(0);
                            var y = train.At(1);
                            scattersData.Add(new ScatterPoint(x, y));
                        }
                        series.Add(new OxyPlot.Wpf.ScatterSeries
                        {
                            ItemsSource = scattersData,
                            Title = $"Training points",
                            MarkerSize = 2
                        });

                        for (int j = 0; j < epochs; j++)
                        {
                            var scattersNeurons = new List<ScatterPoint>();
                            for (int i = 0; i < Som.Neurons.Count; i++)
                            {
                                var x = Som.Neurons[i].HistoryOfWeights[j].At(0);
                                var y = Som.Neurons[i].HistoryOfWeights[j].At(1);
                                scattersNeurons.Add(new ScatterPoint(x, y));
                            }




                            series.Add(new OxyPlot.Wpf.ScatterSeries
                            {
                                ItemsSource = scattersNeurons,
                                Title = $"Neurons",
                                MarkerSize = 4
                            });
                        }
                        ChartWindow chw = new ChartWindow(series);
                        chw.Show();

                    }


                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }


        }
    }
}
