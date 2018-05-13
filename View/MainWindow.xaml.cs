using IAD_zad2.Model;
using IAD_zad2.Utilities.Distance;
using IAD_zad2.Utilities.Generators;
using IAD_zad2.Utilities.NeighbourhoodFunction;
using IAD_zad2.Utilities.Observer;
using IAD_zad2.Utilities.ParametersFunctions;
using MathNet.Numerics.LinearAlgebra;
using OxyPlot.Series;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using IAD_zad2.Model.Parameters;
using IAD_zad2.Utilities.Data.DataProviders;
using IAD_zad2.Utilities.Data.DataProviders.Image;
using IAD_zad2.Utilities.Data.Norm;
using IAD_zad2.Utilities.Error;
using IAD_zad2.Utilities.ImageProcessing;
using Microsoft.Win32;
using OxyPlot.Wpf;
using View.Utility;
using Color = System.Windows.Media.Color;
using Series = OxyPlot.Wpf.Series;

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

        public NeuralNetwork Network { get; set; }

        public List<Vector<double>> TrainingData { get; set; }

        public int NumberOfNeurons { get; set; }
        #region UiToBind
        public string ManhattanChoice { get; set; } = "Manhattan";
        public string EuclideanChoice { get; set; } = "Euclidean";

        public string KohonenGaussianChoice { get; set; } = "Kohonen gaussian";
        public string KohonenBubbleChoice { get; set; } = "Kohonen bubble";
        public string NeuralGasChoice { get; set; } = "Neural gas";
        public string WtaChoice { get; set; } = "WTA";

        public string SomChoice { get; set; } = "Som";
        public string KMeansChoice { get; set; } = "K-means algorithm";
        #endregion

        #region Creators

        private void SomCreate_Click(object sender, RoutedEventArgs eventArgs)
        {
            try
            {
                int neuronCount = Int32.Parse(SomNeuronCount.Text);
                NumberOfNeurons = neuronCount;
                int dimensions = Int32.Parse(SomDimensions.Text);
                string choice = SomDistance.Text;
                IDistanceCalculator dist = new EuclideanDistance();
                if (choice == ManhattanChoice)
                {
                    dist = new ManhattanDistance();
                }
                double minWeights = double.Parse(SomWeightsMin.Text, CultureInfo.InvariantCulture);
                double maxWeights = double.Parse(SomWeightsMax.Text, CultureInfo.InvariantCulture);
                Network = new SelfOrganizingMap(neuronCount, new NeuronRandomRectangularInitializer(minWeights, maxWeights, dimensions), dist);

                if (AlgorithmSelectBox.Text == SomChoice)
                {
                    Network = new SelfOrganizingMap(neuronCount, new NeuronRandomRectangularInitializer(minWeights, maxWeights, dimensions), new EuclideanDistance());
                }
                else
                {
                    Network = new KMeansNetwork(neuronCount, new NeuronRandomRectangularInitializer(minWeights, maxWeights, dimensions), new EuclideanDistance());
                }

                SomInformation.Text =
                    $"Successfully created {AlgorithmSelectBox.Text}\n NC: {neuronCount}; D: {choice}; Min.: {minWeights}; Max.: {maxWeights}";
            }
            catch (Exception e)
            {
                MessageBox.Show($"Something went wrong while parsing. Original message: {e.Message}");
            }
        }


        #endregion Creators




        private void StartTrainButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            if (Network == null || TrainingData == null)
            {
                MessageBox.Show("You didn't create network or get data points!\n");
            }
            else
            {
                try
                {
                    TrainingParameters p = GetParameters();
                    StandardTrainingObserver sno = Network.Dimensions == 2 ? new StandardTrainingObserver() : null;
                    Network.Observer = sno;

                    Network.Train(p);
                    IErrorCalculator err = new QuantizationErrorCalculator()
                    {
                        DistanceCalculator = Network.DistanceCalculator
                    };
                    SomInformation.Text += $"\nSom trained. Quantization error: {err.CalculateError(Network.Neurons, TrainingData):F} ";
                    if (sno != null) DisplayIfTwoDimensional(sno);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void DisplayIfTwoDimensional(StandardTrainingObserver sno)
        {
            if (Network.Dimensions == 2)
            {
                if (SingleSeriesCheckBox.IsChecked == true)
                {
                    List<OxyPlot.Wpf.Series> moving =
                        View.Utility.Converter.ConvertToScatterSeries(sno.HistoryOfNeurons);

                    var scattersData = new List<ScatterPoint>();
                    foreach (var train in TrainingData)
                    {
                        var x = train.At(0);
                        var y = train.At(1);
                        scattersData.Add(new ScatterPoint(x, y));
                    }
                    var stat = new OxyPlot.Wpf.ScatterSeries
                    {
                        ItemsSource = scattersData,
                        Title = $"Training points",
                        MarkerSize = 2,
                        MarkerFill = Color.FromRgb(140, 131, 130)
                    };

                    ChartWindow chw1 = new ChartWindow(stat, moving);
                    chw1.Show();
                }
                if (MultipleSeriesCheckbox.IsChecked == true)
                {
                    List<List<OxyPlot.Wpf.Series>> series = new List<List<Series>>();
                    List<List<Vector<double>>> history = Network.Observer.HistoryOfNeurons;

                    for(int j = 0; j < history.Count; j++)
                    {
                        var epoch = history[j];
                        if (j % 4 == 0)
                        {
                            List<OxyPlot.Wpf.Series> group = new List<Series>();
                            Dictionary<Neuron, List<Vector<double>>> neuronZones =
                                new Dictionary<Neuron, List<Vector<double>>>();
                            int i = 0;
                            foreach (var neuron in Network.Neurons)
                            {
                                neuronZones.Add(neuron, new List<Vector<double>>());
                                neuron.CurrentWeights = epoch[i];
                                i++;
                            }


                            foreach (var data in TrainingData)
                            {
                                neuronZones[Network.GetWinner(data)].Add(data);
                            }


                            foreach (var neuronZone in neuronZones)
                            {
                                var scatterSet = new List<ScatterPoint>();
                                scatterSet.Add(
                                    Converter.ConvertVectorToScatterPoint(neuronZone.Key.CurrentWeights, true));
                                var points = neuronZone.Value;
                                foreach (var point in points)
                                {
                                    scatterSet.Add(Converter.ConvertVectorToScatterPoint(point));
                                }

                                group.Add(new OxyPlot.Wpf.ScatterSeries
                                {
                                    ItemsSource = scatterSet,
                                    Title = $"Class",
                                    MarkerSize = 3
                                });
                            }

                            series.Add(group);
                        }
                    }

                    ChartWindow chw2 = new ChartWindow(series);

                    chw2.Show();
                }
            }

        }


        #region UiObjectGenerators
        private TrainingParameters GetParameters()
        {

            int epochs = Int32.Parse(TrainingEpochs.Text);
            int iterations = Int32.Parse(TrainingIterations.Text);
            TrainingParameters p = p = new KMeansTrainingParameters()
            {
                Epochs = epochs,
                TrainingData = TrainingData
            };

            if (Network is SelfOrganizingMap)
            {
                int kmax = TrainingData.Count * epochs;
                double minLearningRate = double.Parse(TrainingLearningRateMin.Text, CultureInfo.InvariantCulture);
                double maxLearningRate = double.Parse(TrainingLearningRateMax.Text, CultureInfo.InvariantCulture);
                p = new SomTrainingParameters()
                {
                    LearningRate = new DeclineExponentially(kmax, minLearningRate, maxLearningRate),
                    TirednessMechanism = GetTiredMechanism(),
                    NeighbourhoodFunction = GetNeighbourhoodFunction(kmax),
                    NumberOfIterations = iterations,
                    TrainingData = TrainingData
                };
            }

            return p;
        }

        private INeighborhoodFunction GetNeighbourhoodFunction(int kmax)
        {
            double minParameter = double.Parse(TrainingParameterMin.Text, CultureInfo.InvariantCulture);
            double maxParameter = double.Parse(TrainingParameterMax.Text, CultureInfo.InvariantCulture);
            IDecliner decPara = new DeclineExponentially(kmax, minParameter, maxParameter);
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
            else if (choice == WtaChoice)
            {
                fun = new WTANeighbourhoodFunction();
            }

            return fun;
        }

        private ITirednessMechanism GetTiredMechanism()
        {
            if (TrainingTirednessMechanism.IsChecked != null && TrainingTirednessMechanism.IsChecked == true)
            {
                double minPotential = double.Parse(TrainingNeuronPotentialMin.Text, CultureInfo.InvariantCulture);
                double maxPotential = double.Parse(TrainingNeuronPotentialMax.Text, CultureInfo.InvariantCulture);
                if (TrainingTirednessUpperLimit.Text == "")
                {
                    return new PotentialTiredness(maxPotential, minPotential, (1.0 / NumberOfNeurons));
                }
                else
                {
                    int upperBound = Int32.Parse(TrainingTirednessUpperLimit.Text, CultureInfo.InvariantCulture);
                    return new PotentialTiredness(maxPotential, minPotential, (1.0 / NumberOfNeurons), upperBound);
                }
            }

            return null;
        }
        #endregion

        private void NormalizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (TrainingData != null)
            {
                int square = 1;
                INormalizer norm = new ScalingSquareNormalizer(square);
                norm.Normalize(TrainingData);
                DataInfo.Text += $"\nData scaled to range (-{square}, {square})";
            }
            else
            {
                MessageBox.Show("Provide data first!");
            }
        }



        #region UiUpdates
        private void TrainingTirednessMechanism_Unchecked(object sender, RoutedEventArgs e)
        {
            if (TrainingNeuronPotentialMin != null) TrainingNeuronPotentialMin.IsEnabled = false;
            if (TrainingNeuronPotentialMax != null) TrainingNeuronPotentialMax.IsEnabled = false;
            if (TrainingTirednessUpperLimit != null) TrainingTirednessUpperLimit.IsEnabled = false;
        }

        private void TrainingTirednessMechanism_Checked(object sender, RoutedEventArgs e)
        {
            if (TrainingNeuronPotentialMin != null) TrainingNeuronPotentialMin.IsEnabled = true;
            if (TrainingNeuronPotentialMax != null) TrainingNeuronPotentialMax.IsEnabled = true;
            if (TrainingTirednessUpperLimit != null) TrainingTirednessUpperLimit.IsEnabled = true;
        }

        private void TrainingAlghoritmType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && ((ComboBoxItem)(e.AddedItems[0])).Content.ToString().Contains(NeuralGasChoice))
            {
                TrainingParameterMin.IsEnabled = true;
                TrainingParameterMax.IsEnabled = true;
                TrainParameterLabel.Text = "Neural gas lambda";
            }
            else if (e.AddedItems.Count > 0 && ((ComboBoxItem)(e.AddedItems[0])).Content.ToString().Contains("Kohonen"))
            {
                TrainingParameterMin.IsEnabled = true;
                TrainingParameterMax.IsEnabled = true;
                TrainParameterLabel.Text = "Kohonen neighborhood radius";
            }
            else if (e.AddedItems.Count > 0 && ((ComboBoxItem)(e.AddedItems[0])).Content.ToString().Contains(WtaChoice))
            {
                TrainingParameterMin.IsEnabled = false;
                TrainingParameterMax.IsEnabled = false;

                TrainParameterLabel.Text = "Parameter";
            }
        }
        #endregion



        #region DataTab
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

        private void GenerateDataButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            try
            {
                IDataProvider prov = new RandomDataProvider(Int32.Parse(DataPointsCount.Text));
                TrainingData = prov.GetData();
                DataInfo.Text = $"Successfully generated data.\nData dimensions: {TrainingData.First().Count}\nData count: {TrainingData.Count}";
            }
            catch (FormatException e)
            {
                MessageBox.Show($"Something went wrong while parsing. Original message {e.Message}.");
            }
        }


        private void BitmapButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            try
            {
                Nullable<bool> result = openFileDialog1.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    // Open document
                    string filename = openFileDialog1.FileName;
                    DataToCompressFromImageGenerator gen = new DataToCompressFromImageGenerator();

                    gen.GenerateData(filename, System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//bitmap_data.txt");
                    DataInfo.Text = $"Successfully generated data.";
                }
            }

            catch (Exception e)
            {
                MessageBox.Show($"File could not be read. Original message: {e.Message}.");
            }


        }



        private void CompressButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            IImageSomCompressor com = new ImageProcessing();
            try
            {
                com.Compress(Network, TrainingData);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Something went wrong. Original message: {e.Message}.");
            }
        }

        private void DecompressButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            IImageSomCompressor com = new ImageProcessing();
            try
            {
                Bitmap bm = com.Decompress(Network);
                ImageWindow iw = new ImageWindow(bm);
                iw.Show();

            }
            catch (Exception e)
            {
                MessageBox.Show($"Something went wrong. Original message: {e.Message}.");
            }
        }
        #endregion
    }
}