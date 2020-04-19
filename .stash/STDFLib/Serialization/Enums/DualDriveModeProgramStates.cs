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
    public enum DualDriveModeProgramStates
    {
        LowAtD2LowAtD1 = 0,
        LowAtD2HighAtD1 = 1,
        HighAtD2LowAtD1 = 2,
        HighAtD2HighAtD1 = 3,
        CompareLow = 4,
        CompareHigh = 5,
        CompareMidband = 6,
        DoNotCompare = 7
    }
}
