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
    public enum NormalModeProgramStates
    {
        // Normal program states
        DriveLow = 0,
        DriveHigh = 1,
        ExpectLow = 2,
        ExpectHigh = 3,
        ExpectMidband = 4,
        ExpectValid = 5,
        DoNotDrive = 6,
        KeepWindowOpen = 7
    }
}
