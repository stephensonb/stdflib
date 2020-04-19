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
    public class STDFRecordHeader
    {
        public ushort REC_LEN { get; set; }
        public byte REC_TYP { get; set; }
        public byte REC_SUB { get; set; }
        public RecordType RecordType
        {
            get
            {
                return (RecordType)(REC_TYP << 8 | REC_SUB);
            }
        }
    }
}
