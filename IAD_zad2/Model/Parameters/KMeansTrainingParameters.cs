using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAD_zad2.Exceptions;

namespace IAD_zad2.Model.Parameters
{
    public class KMeansTrainingParameters :TrainingParameters
    {
        public int Epochs { get; set; }
        public new void Validate()
        {
            base.Validate();
            if (Epochs < 0) throw new TrainingParametersException("Number of epochs must be greater than zero.");

        }
    }
}
