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
    public enum SCIOModeProgramStates
    {
        DriveLowCompareLow = 0,
        DriveLowCompareHigh = 1,
        DriveLowCompareMidband = 2,
        DriveLowDoNotCompare = 3,
        DriveHighCompareLow = 4,
        DriveHighCompareHigh = 5,
        DriveHighCompareMidband = 6,
        DriveHighDoNotCompare = 7
    }
}
