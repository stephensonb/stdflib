using System;

namespace STDFLib
{
    public interface IByteConverter
    {
        Endianness Endianness { get; }

        byte[] GetBytes(object value);
        void SetEndianness(Endianness endianness);
        BitField ToBitField(byte[] buffer);
        BitField2 ToBitField2(byte[] buffer);
        bool ToBool(byte[] buffer, int start = 0);
        char[] ToCharArray(byte[] buffer, int length, long start = 0);
        DateTime ToDateTime(byte[] buffer, int start = 0);
        double ToDouble(byte[] buffer, int start = 0);
        float ToFloat(byte[] buffer, int start = 0);
        short ToInt16(byte[] buffer, int start = 0);
        int ToInt32(byte[] buffer, int start = 0);
        long ToInt64(byte[] buffer, int start = 0);
        Nibbles ToNibbles(byte[] buffer);
        string ToString(byte[] buffer, int start = 0);
        ushort ToUInt16(byte[] buffer, int start = 0);
        uint ToUInt32(byte[] buffer, int start = 0);
        ulong ToUInt64(byte[] buffer, int start = 0);
    }
}