using System;
using System.Text;

namespace STDFLib2
{
    public interface IFormatterConverter
    {
        bool SwapBytes { get; set; }
        Endianness Endianness { get; }
        Encoding Encoding { get; }
        byte[] GetBytes(object value);
        byte[] GetBytes(object value, Type valueType);
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
        Nibble ToNibble(object value);
        NibbleArray ToNibbleArray(object value);
        ByteArray ToByteArray(object value);
        BitArray ToBitArray(object value);
    }
}
