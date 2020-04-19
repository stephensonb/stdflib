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
    public class GDRSurrogate : STDFRecordSerializationSurrogate
    {
        protected override int GetItemCount(ISTDFRecord record, string itemCountProperty)
        {
            int count = -1;
            if (record is GDR gdr)
            {
                count = itemCountProperty switch
                {
                    "FieldData" => gdr.FLD_CNT,
                    _ => base.GetItemCount(record, itemCountProperty)
                };
            }
            return count;
        }
    }
}
