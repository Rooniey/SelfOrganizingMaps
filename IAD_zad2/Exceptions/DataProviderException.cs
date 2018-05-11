using System;

namespace IAD_zad2.Exceptions
{
    internal class DataProviderException : Exception
    {
        public DataProviderException()
        {
        }

        public DataProviderException(string message)
            : base(message)
        {
        }

        public DataProviderException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}