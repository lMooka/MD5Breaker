using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Core.Exceptions
{
    public class HashFoundException : Exception
    {
        public override string Message
        {
            get
            {
                return message;
            }
        }

        string message;

        public HashFoundException(string message)
        {
            this.message = message;
        }
    }
}
