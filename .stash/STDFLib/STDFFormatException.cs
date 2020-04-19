using System;
using System.Runtime.Serialization;

namespace STDFLib2
{
    public class STDFFormatException : Exception
    {
        public STDFFormatException() : base()
        {
        }

        public STDFFormatException(string message) : base(message)
        {
        }

        public STDFFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected STDFFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
