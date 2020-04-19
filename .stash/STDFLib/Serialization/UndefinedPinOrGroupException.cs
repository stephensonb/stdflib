using System;
using System.Runtime.Serialization;

// Formatter - serializes an object to or from a binary stream
// Converter - converts custom objects to a value of a specific type
// Surrogate - provides custom serialization for an object
// SurrogateSelector - determines which surrogate to use for serialization of custom types
// ByteConverter - converts value types to/from binary representation, honoring endianness

// To setup for serialization
// 
// Formatter = new Formatter(


namespace STDFLib.Serialization
{
    public class UndefinedPinOrGroupException : Exception
    {
        public UndefinedPinOrGroupException()
        {
        }

        public UndefinedPinOrGroupException(string message) : base(message)
        {
        }

        public UndefinedPinOrGroupException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UndefinedPinOrGroupException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
