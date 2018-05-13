using IAD_zad2.Exceptions;
using IAD_zad2.Utilities.NeighbourhoodFunction;
using IAD_zad2.Utilities.ParametersFunctions;

namespace IAD_zad2.Model.Parameters
{
    public class SomTrainingParameters : TrainingParameters
    {
        public INeighborhoodFunction NeighbourhoodFunction { get; set; }

        public IDecliner LearningRate { get; set; }

        public int NumberOfIterations { get; set; }

        public ITirednessMechanism TirednessMechanism { get; set; }

        public new void Validate()
        {
            base.Validate();
            if (NumberOfIterations < 0) throw new TrainingParametersException("Number of iterations must be greater than zero.");
            if (NeighbourhoodFunction == null) throw new TrainingParametersException("Neighbourhood function can not be null.");
            if (LearningRate == null) throw new TrainingParametersException("Learning rate decliner can not be null.");
        }
    }
}
