using System;

namespace STDFLib
{
    public interface ISTDFReader : IDisposable
    {
        STDFCpuTypes CPU_TYPE { get; }
        STDFVersions STDF_VER { get; }
        ushort CurrentRecordLength { get; }
        RecordType CurrentRecordType { get; }
        bool EOF { get; }
        long Position { get; }
        void Close();
        object Read(Type dataType, int dataLength = int.MinValue);
        object Read(Type dataType, int itemCount = 0, int itemDataLength = int.MinValue);
        BitField ReadBitField();
        BitField2 ReadBitField2();
        int ReadByte();
        byte[] ReadBytes(int length);
        DateTime ReadDateTime();
        double ReadDouble();
        void ReadHeader();
        short ReadInt16();
        int ReadInt32();
        long ReadInt64();
        Nibbles ReadNibbles(int nibbleCount = 1);
        float ReadSingle();
        string ReadString(int length);
        ushort ReadUInt16();
        uint ReadUInt32();
        ulong ReadUInt64();
        void Rewind();
        long SeekNextRecord();
        bool SeekNextRecordType(RecordType type);
    }
}