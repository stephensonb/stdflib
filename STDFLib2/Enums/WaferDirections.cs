// Formatter - serializes an object to or from a binary stream
// Converter - converts custom objects to a value of a specific type
// Surrogate - provides custom serialization for an object
// SurrogateSelector - determines which surrogate to use for serialization of custom types
// ByteConverter - converts value types to/from binary representation, honoring endianness

// To setup for serialization
// 
// Formatter = new Formatter(

namespace STDFLib2.Serialization
{
    public enum WaferDirections
    {
        Unknown = 0,
        Down = 1,
        Right = 2,
        Up = 3,
        Left = 4
    }
}
