using System;
using System.IO;
using System.Text;

namespace STDFLib
{
    public class STDFReader : IDisposable, ISTDFReader
    {
        private readonly BinaryReader fs = null;
        private readonly ByteConverter Converter;
        public STDFCpuTypes CPU_TYPE { get; private set; }
        public STDFVersions STDF_VER { get; private set; }
        public ushort CurrentRecordLength { get; private set; }
        public RecordType CurrentRecordType { get; private set; }

        public bool EOF
        {
            get
            {
                return (fs == null || Position >= fs.BaseStream.Length || Position < 0);
            }
        }

        public STDFReader(string path)
        {
            Converter = new ByteConverter();
            fs = new BinaryReader(File.OpenRead(path));
            Rewind();
            ReadHeader();

            // Check that the current record being pointed to is the FAR record type
            if (CurrentRecordType.TypeCode == (ushort)RecordTypes.FAR)
            {
                if (CurrentRecordLength == 512)
                {
                    // FAR record Length should be 2.  If 512, then file was created with BigEndian byte order.
                    // Configure the byte converter to use BigEndian byte ordering for int/float/doubles.
                    Converter.SetEndianness(Endianness.BigEndian);
                }
            }
            else
            {
                throw new FormatException("Invalid STDF format.");
            }
            CPU_TYPE = (STDFCpuTypes)fs.ReadByte();
            STDF_VER = (STDFVersions)fs.ReadByte();
        }

        public void Close()
        {
            fs.Close();
        }

        public void Rewind()
        {
            fs.BaseStream.Seek(0, SeekOrigin.Begin);
        }

        public void ReadHeader()
        {
            byte[] buffer = fs.ReadBytes(4);
            CurrentRecordLength = Converter.ToUInt16(buffer, 0);
            CurrentRecordType = (ushort)(buffer[2] << 8 | buffer[3]);
        }

        public bool SeekNextRecordType(RecordType type)
        {
            bool found = false;

            while (!found)
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

            if (currentPos >= fs.BaseStream.Length || currentPos < 0)
            {
                return -1;
            }

            try
            {
                fs.BaseStream.Seek(CurrentRecordLength + 2, SeekOrigin.Current);
            }
            catch (EndOfStreamException e)
            {
                fs.BaseStream.Seek(0, SeekOrigin.End);
                return -1;
            }

            ReadHeader();

            return Position;
        }

        public long Position
        {
            get
            {
                return fs.BaseStream.Position;
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

        public virtual object Read(Type dataType, int itemCount, int itemDataLength)
        {
            var values = Array.CreateInstance(dataType, itemCount);

            // Fill the array
            for (int i = 0; i < itemCount; i++)
            {
                values.SetValue(Read(dataType, itemDataLength),i);
            }

            return values;
        }

        public short ReadInt16()
        {
            return Converter.ToInt16(fs.ReadBytes(2));
        }

        public ushort ReadUInt16()
        {
            return Converter.ToUInt16(fs.ReadBytes(2));
        }

        public int ReadInt32()
        {
            return Converter.ToInt32(fs.ReadBytes(4));
        }

        public uint ReadUInt32()
        {
            return Converter.ToUInt32(fs.ReadBytes(4));
        }

        public long ReadInt64()
        {
            return Converter.ToInt64(fs.ReadBytes(8));
        }

        public ulong ReadUInt64()
        {
            return Converter.ToUInt64(fs.ReadBytes(8));
        }

        public float ReadSingle()
        {
            return Converter.ToFloat(fs.ReadBytes(4));
        }

        public double ReadDouble()
        {
            return Converter.ToDouble(fs.ReadBytes(8));
        }

        public DateTime ReadDateTime()
        {
            uint date = ReadUInt32();
            return DateTime.UnixEpoch.AddSeconds(date);
        }

        public string ReadString(int length)
        {
            if (length == 0) return "";
            byte[] buffer = fs.ReadBytes(length);
            char[] ca = new char[length];
            Encoding.Convert(Encoding.ASCII, Encoding.Default, buffer).CopyTo(ca, 0);
            return new string(ca);
        }

        public char ReadChar()
        {
            return (char)fs.ReadByte();
        }

        public byte[] ReadBytes(int length)
        {
            return fs.ReadBytes(length);
        }

        public virtual object Read(Type dataType, int dataLength)
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
                case "Char": return ReadChar();
                case "Char[]": return ReadString(dataLength < 0 ? ReadByte() : dataLength).ToCharArray();
                case "String": return ReadString(dataLength < 0 ? ReadByte() : dataLength);
                case "BitField": return ReadBitField();
                case "BitField2": return ReadBitField2();
                case "Nibbles": return ReadNibbles(dataLength < 0 ? 1 : dataLength);
                case "Byte[]": return ReadBytes(dataLength < 0 ? ReadByte() : dataLength);
            }

            return null;
        }

        // Read in an array of nibbles from the stream.
        public Nibbles ReadNibbles(int nibbleCount)
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

            if (byteCount == 0) return b; // If zero, then return, nothing else to read.

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

            if (byteCount == 0) return b; // If zero, then return, nothing else to read.

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
