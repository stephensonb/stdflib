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
    public class DuplicateIndexException : Exception
    {
        public DuplicateIndexException()
        {
        }

        public DuplicateIndexException(string message) : base(message)
        {
        }

        public DuplicateIndexException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicateIndexException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
