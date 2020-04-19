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
    public interface IPartCollection
    {
        void StartPartTest(byte headNumber, byte siteNumber);
        void EndPartTest(byte headNumber, byte siteNumber, short xCoordinate, short yCoordinate,uint testTime,
                         string partId, string partText, byte[] partFix);
        IPartData GetPart();
        IPartData GetLastPart();
    }
}
