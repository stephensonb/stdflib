using STDFLib2.Serialization;
using System.IO;
using System;

namespace STDFLib2
{
    public interface ISTDFBinaryWriter
    {
        Stream BaseStream { get; }
        ISTDFFormatterConverter Converter { get; }

        long Position { get; }
        void Flush();
        long Seek(int offset, SeekOrigin seekOrigin);
        void Write(Array values);
        void Write(Array values, int index, int count);
        void Write(byte[] values);
        void Write(byte[] values, int index, int count);
        void Write(byte value);
        void Write(double value);
        void Write(float value);
        void Write(int value);
        void Write(long value);
        void Write(sbyte value);
        void Write(short value);
        void Write(string value);
        void Write(uint value);
        void Write(ulong value);
        void Write(ushort value);
        void Write(BitField value);
        void Write(BitField2 value);
        void Write(Nibbles value);
        void Write(Type dataType, object value);
        void Write(VarDataField value);
    }
}