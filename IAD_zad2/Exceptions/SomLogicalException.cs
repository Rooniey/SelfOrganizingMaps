using System;

namespace IAD_zad2.Exceptions
{
    public class SomLogicalException : Exception
    {
        public SomLogicalException()
        {
        }

        public SomLogicalException(string message)
            : base(message)
        {
        }

        public SomLogicalException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}