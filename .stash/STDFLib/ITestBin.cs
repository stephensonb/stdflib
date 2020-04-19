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
    public interface ITestBin
    {
        byte HEAD_NUM { get; set; }
        byte SITE_NUM { get; set; }
        ushort BIN_NUM { get; set; }
        uint BIN_CNT { get; set; }
        char BIN_PF { get; set; }
        string BIN_NAM { get; set; }
    }
}
