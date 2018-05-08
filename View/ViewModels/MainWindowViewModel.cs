using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.ViewModels
{
    class MainWindowViewModel
    {
        public int EpochsCount { get; set; }
        public int NeuronCount { get; set; }
        public int PointsCount { get; set; }
        public double LearningRateMAX { get; set; }
        public double LearningRateMIN { get; set; }
        public double KohonenNeighRadMAX { get; set; }
        public double KohonenNeighRadMIN { get; set; }
    }
}
