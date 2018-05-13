using System;

namespace IAD_zad2.Exceptions
{
    public class TrainingParametersException : Exception
    {
         public TrainingParametersException()
        {
        }

        public TrainingParametersException(string message)
            : base(message)
        {
        }

        public TrainingParametersException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
