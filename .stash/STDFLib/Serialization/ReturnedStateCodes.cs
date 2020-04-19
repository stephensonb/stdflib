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
    public enum ReturnedStateCodes
    {
        Low = 0,
        High = 1,
        Midband = 2,
        Glitch = 3,
        Undetermined = 4,
        FailedLow = 5,
        FailedHigh = 6,
        FailedMidband = 7,
        FailedWithGlitch = 8,
        Open = 9,
        Short = 10
    }
}
