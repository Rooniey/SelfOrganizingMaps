using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAD_zad2.Utilities.Data
{
    class DataProviderException : Exception
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
