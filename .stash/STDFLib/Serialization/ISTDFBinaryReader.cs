using STDFLib2.Serialization;
using System;
using System.IO;

namespace STDFLib2
{
    public interface ISTDFBinaryReader : IDisposable
    {
        ISTDFFormatterConverter Converter { get; }
        Stream BaseStream { get; }
        object Read(Type dataType);
        Array Read(Array values, int index, int count);
        object[] Read(Type elementType, int count);
        byte ReadByte();
        byte[] ReadBytes(int count);
        char ReadChar();
        char[] ReadChars(int count);
        double ReadDouble();
        short ReadInt16();
        int ReadInt32();
        long ReadInt64();
        sbyte ReadSByte();
        float ReadSingle();
        string ReadString();
        ushort ReadUInt16();
        uint ReadUInt32();
        ulong ReadUInt64();
        BitField ReadBitField();
        BitField2 ReadBitField2();
        Nibbles ReadNibbles(int nibbleCount);
        VarDataField ReadVarDataField();
    }
}