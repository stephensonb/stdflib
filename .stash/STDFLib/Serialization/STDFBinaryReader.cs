using STDFLib2.Serialization;
using System;
using System.IO;
using System.Text;

namespace STDFLib2
{
    public class STDFBinaryReader : ISTDFBinaryReader
    {
        protected bool LeaveOpen = false;

        public ISTDFFormatterConverter Converter { get; set; }
        public Stream BaseStream { get; private set; }

        public STDFBinaryReader(Stream stream) : this(stream, new STDFFormatterConverter(), false) { }
        public STDFBinaryReader(Stream stream, ISTDFFormatterConverter converter) : this(stream, converter, false) { }
        public STDFBinaryReader(Stream stream, ISTDFFormatterConverter converter, bool leaveOpen) 
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if (!stream.CanRead)
            {
                throw new InvalidOperationException("Stream passed to STDFBinaryReader is not readable.");
            }
            BaseStream = stream;
            LeaveOpen = leaveOpen;
            Converter = converter;
        }

        public void Close()
        {
            BaseStream.Close();
        }

        public  Array Read(Array values, int index, int count)
        {
            if (values == null) 
            {
                throw new ArgumentNullException("values");
            }

            if ((count+index) > values.Length)
            {
                throw new IndexOutOfRangeException("Invalid index and count combination greater than length of target array.");
            }
            
            Type elementType = values.GetType().GetElementType();

            for(int i=0;i<count;i++)
            {
                values.SetValue(Read(elementType), i+index);
            }

            return values;
        }

        public object[] Read(Type elementType, int count)
        {
            if (count < 0)
            {
                throw new IndexOutOfRangeException("Element count must be greater >= to zero");
            }

            object[] values = new object[count];
            //Array values = Array.CreateInstance(elementType, count);
            for (int i = 0; i < count; i++)
            {
                values[i] = Read(elementType);
            }
            
            return values;
        }

        public bool ReadBoolean()
        {
            return ReadByte() == 0 ? false : true;
        }

        public byte ReadByte()
        {
            return (byte)BaseStream.ReadByte();
        }

        public byte[] ReadBytes(int count)
        {
            byte[] buffer = new byte[count];
            BaseStream.Read(buffer, 0, count);
            return buffer;
        }

        public sbyte ReadSByte()
        {
            return (sbyte)ReadByte();
        }

        public char ReadChar()
        {
            return ReadChars(1)[0];
        }

        public char[] ReadChars(int count)
        {
            byte[] chr = ReadBytes(count);
            return Encoding.ASCII.GetChars(chr);
        }

        // First byte has length of string (max 255 characters)
        public string ReadString()
        {
            int length = ReadByte();
            if (length == 0)
            {
                return "";
            }
            return new string(ReadChars(length));
        }

        public double ReadDouble() => Converter.ToDouble(ReadBytes(sizeof(double)));
        public float ReadSingle() => Converter.ToSingle(ReadBytes(sizeof(float)));
        public short ReadInt16() => Converter.ToInt16(ReadBytes(sizeof(short)));
        public ushort ReadUInt16() => Converter.ToUInt16(ReadBytes(sizeof(ushort)));
        public int ReadInt32() => Converter.ToInt32(ReadBytes(sizeof(int)));
        public uint ReadUInt32() => Converter.ToUInt32(ReadBytes(sizeof(uint)));
        public long ReadInt64() => Converter.ToInt64(ReadBytes(sizeof(long)));
        public ulong ReadUInt64() => Converter.ToUInt64(ReadBytes(sizeof(ulong)));
        public BitField ReadBitField()
        {
            int length = ReadByte();
            if (length == 0)
            {
                return new BitField();
            }
            return new BitField(ReadBytes(length));
        }
        public BitField2 ReadBitField2()
        {
            int bitCount = ReadUInt16();
            if (bitCount == 0)
            {
                return new BitField2();
            }
            return new BitField2(ReadBytes((bitCount / 8) + ((bitCount % 8) > 0 ? 1 : 0)));
        }
        public Nibbles ReadNibbles(int nibbleCount)
        {
            if (nibbleCount == 0)
            {
                return new Nibbles();
            }
            return new Nibbles(ReadBytes(nibbleCount / 2 + (nibbleCount % 2)));
        }
        public object Read(Type dataType)
        {
            string typeName = (dataType.Name == "Nullable`1") ? dataType.GenericTypeArguments[0].Name : dataType.Name;

            // write the field
            return typeName switch
            {
                "Boolean" => ReadBoolean(),
                "DateTime" => DateTime.UnixEpoch.AddSeconds(ReadUInt32()),
                "Byte" => ReadByte(),
                "SByte" => ReadSByte(),
                "Char" => ReadChar(),
                "Int16" => ReadInt16(),
                "Int32" => ReadInt32(),
                "UInt16" => ReadUInt16(),
                "UInt32" => ReadUInt32(),
                "Single" => ReadSingle(),
                "Double" => ReadDouble(),
                "String" => ReadString(),
                "BitField" => ReadBitField(),
                "BitField2" => ReadBitField2(),
                "VarDataField" => ReadVarDataField(),
                _ => null,
            };
        }

        public VarDataField ReadVarDataField()
        {
            // read the type code for the next field in the stream
            byte fieldTypeCode = ReadByte();

            // if the field started on an even byte, check for a padding byte (code = 0)
            if (fieldTypeCode == 0)
            {
                // padding byte was found, so read the next byte for the actual field type.
                fieldTypeCode = ReadByte();
            }

            return fieldTypeCode switch
            {
                1 => (VarDataField)ReadByte(),
                2 => (VarDataField)ReadUInt16(),
                3 => (VarDataField)ReadUInt32(),
                4 => (VarDataField)ReadSByte(),
                5 => (VarDataField)ReadInt16(),
                6 => (VarDataField)ReadInt32(),
                7 => (VarDataField)ReadSingle(),
                8 => (VarDataField)ReadDouble(),
                10 => (VarDataField)ReadString(),
                11 => (VarDataField)ReadBitField(),
                12 => (VarDataField)ReadBitField2(),
                13 => (VarDataField)ReadNibbles(1),
                _ => throw new STDFFormatException(string.Format("Invalid file format or unsupported Generic Record field data type found in data stream.  Field data type code '{0}' found at position {1}", fieldTypeCode, BaseStream.Position)),
            };
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (!LeaveOpen)
                    {
                        BaseStream.Dispose();
                        BaseStream = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~STDFBinaryReader()
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
