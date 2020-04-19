using System;
using System.Text;

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
    public interface ISTDFFormatterConverter
    {
        bool SwapBytes { get; set; }
        Endianness Endianness { get; }
        Encoding Encoding { get; }
        byte[] GetBytes(object value);
        object Convert(object value, Type toType);
        bool ToBoolean(object value);
        byte ToByte(object value);
        char ToChar(object value);
        DateTime ToDateTime(object value);
        double ToDouble(object value);
        short ToInt16(object value);
        int ToInt32(object value);
        sbyte ToSByte(object value);
        float ToSingle(object value);
        string ToString(object value);
        ushort ToUInt16(object value);
        uint ToUInt32(object value);
    }
}
