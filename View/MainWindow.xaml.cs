using IAD_zad2.Model;
using IAD_zad2.Utilities.Data;
using IAD_zad2.Utilities.Distance;
using IAD_zad2.Utilities.Generators;
using IAD_zad2.Utilities.NeighbourhoodFunction;
using IAD_zad2.Utilities.Observer;
using IAD_zad2.Utilities.ParametersFunctions;
using MathNet.Numerics.LinearAlgebra;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IAD_zad2.Utilities.Data.DataProviders;
using IAD_zad2.Utilities.Data.Norm;

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
        public string KMeansChoice { get; set; } = "K-means";

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


        #endregion Creators




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
                    IDecliner decPara = new DeclineExponentially(TrainingData.Count * epochs, minParameter, maxParameter);
                    IDecliner learnPara = new DeclineExponentially(TrainingData.Count * epochs, minLearningRate, maxLearningRate);
                    ITirednessMechanism tired = null;
                    if (TrainingTirednessMechanism.IsChecked != null && TrainingTirednessMechanism.IsChecked == true)
                    {
                        double minPotential = double.Parse(TrainingNeuronPotentialMin.Text, CultureInfo.InvariantCulture);
                        double maxPotential = double.Parse(TrainingNeuronPotentialMax.Text, CultureInfo.InvariantCulture);
                        if (TrainingTirednessUpperLimit.Text == "")
                        {
                            tired = new PotentialTiredness(maxPotential, minPotential, (1.0 / Som.Neurons.Count));
                        }
                        else
                        {
                            int upperBound = Int32.Parse(TrainingTirednessUpperLimit.Text, CultureInfo.InvariantCulture);
                            tired = new PotentialTiredness(maxPotential, minPotential, (1.0 / Som.Neurons.Count), upperBound);
                        }
                    }
                    StandardNeuronObserver sno = Som.Dimensions == 2 ? new StandardNeuronObserver() : null;
                    Som.Observer = sno;

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
                    else if (choice == KMeansChoice)
                    {
                        fun = new KMeansNeighbourhoodFunction();
                    }
                    Som.Train(TrainingData, epochs, fun, learnPara, tired);
                    SomInformation.Text += "\nSom trained.";

                    if (Som.Dimensions == 2)
                    {
                        List<OxyPlot.Wpf.Series> series =
                            View.Utility.Converter.ConvertToScatterSeries(sno?.HistoryOfNeurons);

                        var scattersData = new List<ScatterPoint>();
                        foreach (var train in TrainingData)
                        {
                            var x = train.At(0);
                            var y = train.At(1);
                            scattersData.Add(new ScatterPoint(x, y));
                        }
                        series.Insert(0, new OxyPlot.Wpf.ScatterSeries
                        {
                            ItemsSource = scattersData,
                            Title = $"Training points",
                            MarkerSize = 2
                        });

                        ChartWindow chw = new ChartWindow(series, TrainingData.Count);
                        chw.Show();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

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
        }

        private void TrainingTirednessMechanism_Checked(object sender, RoutedEventArgs e)
        {
            if (TrainingNeuronPotentialMin != null) TrainingNeuronPotentialMin.IsEnabled = true;
            if (TrainingNeuronPotentialMax != null) TrainingNeuronPotentialMax.IsEnabled = true;
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
            else if (e.AddedItems.Count > 0 && ((ComboBoxItem)(e.AddedItems[0])).Content.ToString().Contains(KMeansChoice))
            {
                TrainingParameterMin.IsEnabled = false;
                TrainingParameterMax.IsEnabled = false;

                TrainParameterLabel.Text = "Parameter";
            }
        }
        #endregion


        private void BitmapButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            IDataProvider dataProvider = new BitmapFileDataProvider();
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
    }
}