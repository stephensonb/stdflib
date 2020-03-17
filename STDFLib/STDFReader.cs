using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace STDFLib
{
    public enum SeekDirection 
    {
        Next,
        Previous,
        First,
        Last,
        End,
        Start
    }

    public class RecordType
    {
        public ushort TypeCode;
        public byte REC_TYP { get => (byte)((TypeCode & 0xFF00) >> 8); }
        public byte REC_SUB { get => (byte)(TypeCode & 0x00FF); }

        public static implicit operator RecordType(int v)
        {
            return new RecordType() { TypeCode = (ushort)v };
        }
    }

    public enum Endianness
    {
        LittleEndian=0,
        BigEndian=1
    }

    public class ByteConverter
    {
        Endianness endianness;

        public ByteConverter()
        {
            this.endianness = BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;
        }

        public void SetEndianness(Endianness endianness)
        {
            this.endianness = endianness;
        }

        public short ToInt16(byte[] buffer, int start=0)
        {
            return (short)ToUInt16(buffer, start);
        }

        public ushort ToUInt16(byte[] buffer, int start=0)
        {
            return (ushort)ToUInt(buffer, start, 2);
        }

        public int ToInt32(byte[] buffer, int start = 0)
        {
            return (int)ToUInt(buffer, start, 4);
        }

        public uint ToUInt32(byte[] buffer, int start = 0)
        {
            return (uint)ToUInt(buffer, start, 4);
        }

        public long ToInt64(byte[] buffer, int start = 0)
        {
            return (long)ToUInt(buffer, start, 8);
        }

        public ulong ToUInt64(byte[] buffer, int start = 0)
        {
            return (uint)ToUInt(buffer, start, 8);
        }

        private ulong ToUInt(byte[] buffer, int start, int length)
        {
            ulong result = 0;
            for(int i=0;i<length;i++)
            {
                if (endianness == Endianness.BigEndian)
                {
                    result = (result << 8) | buffer[i];
                } else
                {
                    result = (result << 8) | buffer[length - i - 1];
                }
            }
            return result;
        }

        private void ReverseBytes(byte[] buffer, int length, int start= 0)
        {
            byte[] rbuff = new byte[length];
            for(int i=0;i<length;i++)
            {
                rbuff[i] = buffer[start + length - i - 1];
            }
            rbuff.CopyTo(buffer, start);
        }

        public float ToFloat(byte[] buffer, int start=0)
        {
            if(endianness == Endianness.BigEndian)
            {
                ReverseBytes(buffer, 4,start);
            }
            return BitConverter.ToSingle(buffer, start);
        }

        public double ToDouble(byte[] buffer, int start=0)
        {
            if(endianness == Endianness.BigEndian)
            {
                ReverseBytes(buffer, 8, start);
            }
            return BitConverter.ToDouble(buffer, start);
        }
    }

    public class STDFReader : IDisposable
    {
        private readonly FileStream fs = null;
        private readonly ByteConverter Converter;
        public STDFCpuTypes CPU_TYPE { get; private set; }
        public STDFVersions STDF_VER { get; private set; }
        public ushort CurrentRecordLength { get; private set; }
        public RecordType CurrentRecordType { get; private set; }

        public bool EOF
        {
            get
            {
                return (fs == null || Position >= fs.Length || Position < 0);
            }
        }

        public STDFReader(string path)
        {
            Converter = new ByteConverter();
            fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            Rewind();

            // Check that the current record being pointed to is the FAR record type
            if (CurrentRecordType.TypeCode == (ushort)RecordTypes.FAR)
            {
                if (CurrentRecordLength == 512)
                {
                    // FAR record Length should be 2.  If 512, then file was created with BigEndian byte order.
                    // Configure the byte converter to use BigEndian byte ordering for int/float/doubles.
                    Converter.SetEndianness(Endianness.BigEndian);
                }
            } else
            {
                throw new FormatException("Invalid STDF format.");
            }
            CPU_TYPE = (STDFCpuTypes)fs.ReadByte();
            STDF_VER = (STDFVersions)fs.ReadByte();
        }

        public void Rewind()
        {
            fs.Seek(0, SeekOrigin.Begin);
            ReadHeader();
        }

        public void ReadHeader()
        {
            byte[] buffer = new byte[4];
            fs.Read(buffer, 0, 4);
            CurrentRecordLength = Converter.ToUInt16(buffer, 0);
            CurrentRecordType = (ushort)(buffer[2] << 8 | buffer[3]);
        }

        public bool SeekNextRecordType(RecordType type)
        {
            bool found = false;
 
            while(!found)
            {
                SeekNextRecord();
                if (CurrentRecordType.TypeCode == type.TypeCode)
                {
                    found = true;
                }
            }

            return found;
        }

        public long SeekNextRecord()
        {
            long currentPos = Position;
            
            if (currentPos >= fs.Length || currentPos < 0)
            {
                return -1;
            }

            try
            {
                fs.Seek(CurrentRecordLength+2, SeekOrigin.Current);
            } catch(EndOfStreamException e)
            {
                fs.Seek(0, SeekOrigin.End);
                return -1;
            }

            ReadHeader();

            return Position;
        }

        public long Position
        {
            get
            {
                return fs.Position;
            }
        }

        public int ReadByte()
        {
            if (!EOF)
            {
                return fs.ReadByte();
            }

            throw new EndOfStreamException();
        }

        public virtual object[] Read(Type dataType, int itemCount = 0, int itemDataLength = int.MinValue)
        {
            // Create an array of the specified elementType and size of itemCount
            object[] values = (object[])Array.CreateInstance(dataType, itemCount);

            // Fill the array
            for (int i = 0; i < itemCount; i++)
            {
                values[i] = Read(dataType, itemDataLength);
            }

            return values;
        }

        public short ReadInt16()
        {
            byte[] buffer = new byte[2];
            fs.Read(buffer, 0, 2);
            return Converter.ToInt16(buffer);
        }

        public ushort ReadUInt16()
        {
            byte[] buffer = new byte[2];
            fs.Read(buffer, 0, 2);
            return Converter.ToUInt16(buffer);
        }

        public int ReadInt32()
        {
            byte[] buffer = new byte[4];
            fs.Read(buffer, 0, 4);
            return Converter.ToInt32(buffer);
        }

        public uint ReadUInt32()
        {
            byte[] buffer = new byte[4];
            fs.Read(buffer, 0, 4);
            return Converter.ToUInt32(buffer);
        }

        public long ReadInt64()
        {
            byte[] buffer = new byte[8];
            fs.Read(buffer, 0, 8);
            return Converter.ToInt64(buffer);
        }

        public ulong ReadUInt64()
        {
            byte[] buffer = new byte[8];
            fs.Read(buffer, 0, 8);
            return Converter.ToUInt64(buffer);
        }

        public float ReadSingle()
        {
            byte[] buffer = new byte[4];
            fs.Read(buffer, 0, 4);
            return Converter.ToFloat(buffer);
        }

        public double ReadDouble()
        {
            byte[] buffer = new byte[8];
            fs.Read(buffer, 0, 8);
            return Converter.ToDouble(buffer);
        }

        public DateTime ReadDateTime()
        {
            uint date = ReadUInt32();
            return DateTime.UnixEpoch.AddSeconds(date);
        }

        public char[] ReadString(int length) 
        {
            if (length == 0) return new char[] { };
            byte[] buffer = new byte[length];
            fs.Read(buffer, 0, length);
            char[] ca = new char[length];
            Encoding.Convert(Encoding.ASCII, Encoding.Default, buffer).CopyTo(ca,0);
            return ca;
        }

        public byte[] ReadBytes(int length)
        {
            if (length == 0)
            {
                return new byte[] { };
            }

            byte[] buffer = new byte[length];
            fs.Read(buffer, 0, length);
            return buffer;
        }

        public virtual object Read(Type dataType, int dataLength = int.MinValue)
        {
            switch (dataType.Name)
            {
                case "Byte": return (byte)ReadByte();
                case "SByte": return (sbyte)ReadByte();
                case "Int16": return ReadInt16();
                case "Int32": return ReadInt32();
                case "UInt16": return ReadUInt16();
                case "UInt32": return ReadUInt32();
                case "Single": return ReadSingle();
                case "Double": return ReadDouble();
                case "DateTime": return ReadDateTime();
                case "Char[]":
                case "String":
                    if (dataLength < 0)
                    {
                        // Assume 1st byte has length of array to read
                        dataLength = ReadByte();
                    }
                    if (dataType.Name == "String")
                    {
                        return ReadString(dataLength).ToString();
                    }
                    return ReadString(dataLength);
                case "BitField": return ReadBitField();
                case "BitField2": return ReadBitField2();
                case "Nibbles": return ReadNibbles(1);
                case "Byte[]":
                    if (dataLength < 0)
                    {
                        dataLength = ReadByte();
                    }
                    return ReadBytes(dataLength);
            }

            return null;
        }

        // Read in an array of nibbles from the stream.
        public Nibbles ReadNibbles(int nibbleCount = 1)
        {
            Nibbles nArray = new Nibbles();

            if (nibbleCount > 0)
            {
                // number of bytes to read are nibble count / 2 + 1.  Each byte contains two nibbles
                nArray.AddRange(ReadBytes(nibbleCount / 2 + 1));
            }

            return nArray;
        }

        // Reads a number of bytes (length specified by first byte) into a bitfield.  Length byte is the number of bytes to read.
        public BitField ReadBitField()
        {
            BitField b = new BitField();
            
            // number of bytes to read for the bitfield
            int byteCount = ReadByte();

            if (byteCount == 0) return null; // If zero, then return, nothing else to read.

            // read the bytes into the bitfield
            b.SetBits(ReadBytes(byteCount));

            return b;
        }

        // Reads a number of bits into the bitfield.  The number of bits in the bitfield specified is by the first 2 bytes.  this is converted to a byte count (divide by 8) then the
        // byte count number of bytes are read into the bitfield.
        public BitField2 ReadBitField2()
        {
            BitField2 b = new BitField2();
            int bitCount = ReadUInt16();

            // Convert the number of bits to read to bytes.  Formula note: Must add 1 to byte count (# bits / 8) unless # of bits is factor of 8, then don't add 1 (on a byte boundary)
            int byteCount = (bitCount / 8) + ((bitCount % 8) > 0 ? 1 : 0);

            if (byteCount == 0) return null; // If zero, then return, nothing else to read.

            // read the bytes into bitfield
            b.SetBits(ReadBytes(byteCount));

            return b;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    fs.Close();
                    fs.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~STDFReader()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }


}
