using System.Collections.Generic;

// Formatter - serializes an object to or from a binary stream
// Converter - converts custom objects to a value of a specific type
// Surrogate - provides custom serialization for an object
// SurrogateSelector - determines which surrogate to use for serialization of custom types
// ByteConverter - converts value types to/from binary representation, honoring endianness

namespace STDFLib2.Serialization
{
    public interface IPartData
    {
        PIR PartInfo { get; set; }
        List<ITestResult> TestResults { get; }
        PRR PartResults { get; set; }
    }
}
