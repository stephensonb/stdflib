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
    public enum ReturnedStates
    {
        Low = 0x00,
        High = 0x01,
        Midband = 0x02,
        Glitch = 0x03,
        Undetermined = 0x04,
        FailedLow = 0x05,
        FailedHigh = 0x06,
        FailedMidband = 0x07,
        FailedWithGlitch = 0x08,
        Open = 0x09,
        Short = 0x0A
    }
}
