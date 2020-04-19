using System;
using System.IO;
using System.Text;

namespace STDFLib2
{
    public class STDFBinaryFormatter : IFormatter, IDisposable
    {
        public STDFBinaryFormatter() 
        {
            Converter = new STDFFormatterConverter();
            TypeSurrogateSelector = new SurrogateSelector();
            Buffer = new MemoryStream(4096);
        }

        public  virtual object Deserialize(Stream stream) 
        {
            return null;
        }
        public  virtual void Serialize(Stream stream, object obj) 
        {
            
        }
        public IFormatterConverter Converter { get; set; }
        public bool EndOfStream { get; protected set; }
        protected Stream SerializeStream { get; set; }
        protected SurrogateSelector TypeSurrogateSelector { get; set; }
        protected MemoryStream Buffer { get; }
        protected object ReadArray(Type type, int count)
        {
            if (count <= 0)
            {
                return Array.CreateInstance(type, 0);
            }

            if (type.Name == "NibbleArray")
            {
                type = typeof(byte);
            }

            Array newArray = Array.CreateInstance(type, count);

            for (int i=0;i<count;i++)
            {
                newArray.SetValue(Read(type), i);
            }
            return newArray;
        }
        protected bool ReadBoolean()
        {
            byte val = (byte)SerializeStream.ReadByte();
            return val == 0 ? false : true;
        }
        protected BitArray ReadBitArray()
        {
            int length = Converter.ToUInt16(ReadBytes(2));

            if (length == 0)
            {
                return null;
            }
            // length is number of bits, so divide by 8 to get number of bytes to read.
            return ReadBytes(length/8);
        }
        protected byte ReadByte()
        {
            return (byte)SerializeStream.ReadByte();
        }
        protected ByteArray ReadByteArray()
        {
            int length = ReadByte();

            if (length == 0)
            {
                return null;
            }
            return ReadBytes(length);
        }
        protected byte[] ReadBytes(int count)
        {
            byte[] buffer = new byte[count];
            SerializeStream.Read(buffer, 0, count);
            return buffer;
        }
        protected char ReadChar()
        {
            return Converter.ToChar(SerializeStream.ReadByte());
        }
        protected DateTime ReadDateTime()
        {
            // DateTime stored as unsigend integer (4 bytes)
            return Converter.ToDateTime(ReadBytes(4));
        }
        protected double ReadDouble()
        {
            return Converter.ToDouble(ReadBytes(8));
        }
        protected short ReadInt16()
        {
            return Converter.ToInt16(ReadBytes(2));
        }
        protected int ReadInt32()
        {
            return Converter.ToInt32(ReadBytes(4));
        }
        protected sbyte ReadSByte()
        {
            return (sbyte)ReadByte();
        }
        protected float ReadSingle()
        {
            return Converter.ToSingle(ReadBytes(4));
        }
        protected string ReadString()
        {
            int length = SerializeStream.ReadByte();
            byte[] buffer = ReadBytes(length);
            // special empty string handling.  String of length 1 with the first character value = zero
            // represents null value.
            if (length == 1 && buffer[0] == 0)
            {
                return null;
            }
            return Encoding.ASCII.GetString(buffer);
        }
        protected ushort ReadUInt16()
        {
            return Converter.ToUInt16(ReadBytes(2));
        }
        protected uint ReadUInt32()
        {
            return Converter.ToUInt32(ReadBytes(4));
        }
        protected VarDataField ReadVarDataField()
        {
            // read the type code for the next field in the stream
            GenericDataTypes fieldTypeCode = (GenericDataTypes)ReadByte();
            // if the field started on an even byte, check for a padding byte (code = 0)
            if (fieldTypeCode == 0)
            {
                // padding byte was found, so read the next byte for the actual field type.
                fieldTypeCode = (GenericDataTypes)ReadByte();
            }
            return fieldTypeCode switch
            {
                GenericDataTypes.Byte => (VarDataField)ReadByte(),
                GenericDataTypes.UInt16 => (VarDataField)ReadUInt16(),
                GenericDataTypes.UInt32 => (VarDataField)ReadUInt32(),
                GenericDataTypes.SByte => (VarDataField)ReadSByte(),
                GenericDataTypes.Int16 => (VarDataField)ReadInt16(),
                GenericDataTypes.Int32 => (VarDataField)ReadInt32(),
                GenericDataTypes.Float => (VarDataField)ReadSingle(),
                GenericDataTypes.Double => (VarDataField)ReadDouble(),
                GenericDataTypes.String => (VarDataField)ReadString(),
                GenericDataTypes.ByteArray => (VarDataField)ReadByteArray(),
                GenericDataTypes.BitArray => (VarDataField)ReadBitArray(),
                GenericDataTypes.Nibble => (VarDataField)ReadByte(),
                _ => throw new Exception(string.Format("Invalid file format or unsupported Generic Record field data type found in data stream.  Field data type code '{0}' found at position {1}", fieldTypeCode, SerializeStream.Position)),
            };
        }
        protected object Read(Type type)
        {
            if (type.Name == "Nullable`1")
            {
                type = type.GetGenericArguments()[0];
            }
            return type.Name switch
            {
                "Boolean" => ReadBoolean(),
                "BitArray" => ReadBitArray(),
                "Byte" => ReadByte(),
                "ByteArray" => ReadByteArray(),
                "Char" => ReadChar(),
                "DateTime" => ReadDateTime(),
                "Double" => ReadDouble(),
                "Int16" => ReadInt16(),
                "Int32" => ReadInt32(),
                "SByte" => ReadSByte(),
                "Single" => ReadSingle(),
                "String" => ReadString(),
                "UInt16" => ReadUInt16(),
                "UInt32" => ReadUInt32(),
                "VarDataField" => ReadVarDataField(),
                _ => throw new Exception("Trying to read unsupported type.")
            };
        }
        protected void WriteArray(object obj, Type memberType)
        {
            if (obj is Array values)
            {
                WriteArray(obj, memberType, 0, values.Length);
            }
        }
        protected void Write(byte[] buffer, int startIndex, int length) => SerializeStream.Write(buffer, startIndex, length);
        protected void Write(byte[] buffer) => Write(buffer, 0, buffer.Length);
        protected void WriteArray(object obj, Type memberType, int startIndex, int length)
        {
            if (obj is Array values)
            {
                if (memberType.Name == "Byte")
                {
                    ;
                }
                if (startIndex + length > values.Length)
                {
                    throw new IndexOutOfRangeException("Invalid start index and length combination in WriteArray.");
                }

                if (memberType.Name == "VarDataField")
                {
                    WriteVarDataArray((VarDataField[])obj, startIndex, length);
                } 
                else
                {
                    for (int i = 0; i < length; i++)
                    {
                        WriteMember(values.GetValue(i+startIndex), memberType);
                    }
                }
            }
        }
        protected void WriteVarDataArray(VarDataField[] varData, int startIndex, int length)
        {
            long writeStartPosition = SerializeStream.Position;

            if (startIndex + length > varData.Length)
            {
                throw new IndexOutOfRangeException("Invalid start index and length combination in WriteVarDataArray.");
            }

            for (int i = 0;i < varData.Length; i++)
            {
                int byteOffset = (int)(SerializeStream.Position - writeStartPosition);
                WriteVarDataField(varData[i], byteOffset);
            }
        }
        // Handle reference/nullable types
        protected void WriteBitArray(BitArray val) => Write(Converter.GetBytes(val, typeof(BitArray)));
        protected void WriteByteArray(ByteArray val) => Write(Converter.GetBytes(val, typeof(ByteArray)));
        protected void WriteNibbleArray(NibbleArray val) => Write(Converter.GetBytes(val, typeof(NibbleArray)));
        protected void WriteDateTime(DateTime val) => Write(Converter.GetBytes(val, typeof(DateTime)));
        protected void WriteVarDataField(VarDataField val, int byteOffset)
        {
            // Handle adding padding byte if neccesary.  NOTE: byteOffset is the byte position relative
            // to the starting byte position a Generic Data Record (GDR)
            bool evenByte = (byteOffset % 2) == 0 ? true : false;
            if (evenByte)
            {
                // If we are on an even byte in the stream and we are writing one of the 
                // following types, then we need to add a padding byte as these need to start writing on an even byte
                // (writing the type code would leave the stream position starting on an odd byte)
                switch (val.TypeCode)
                {
                    case GenericDataTypes.Int16:
                    case GenericDataTypes.Int32:
                    case GenericDataTypes.UInt16:
                    case GenericDataTypes.UInt32:
                    case GenericDataTypes.Float:
                    case GenericDataTypes.Double:
                        // write a padding byte
                        WriteByte(0);
                        break;
                }
            }
            // write the field type code
            WriteByte((byte)val.TypeCode);
            // write the field value;
            Write(Converter.GetBytes(val.Value));
        }

        // Handle value types
        protected void WriteBoolean(bool val) => SerializeStream.WriteByte((byte)(val ? 1 : 0));
        protected void WriteByte(byte val) => SerializeStream.WriteByte(val);
        protected void WriteChar(char val) => Write(Converter.GetBytes(val));
        protected void WriteDouble(double val) => Write(Converter.GetBytes(val));
        protected void WriteInt16(short val) => Write(Converter.GetBytes(val));
        protected void WriteInt32(int val) => Write(Converter.GetBytes(val));
        protected void WriteSByte(sbyte val) => SerializeStream.WriteByte((byte)val);
        protected void WriteSingle(float val) => Write(Converter.GetBytes(val));
        protected void WriteString(string val) => Write(Converter.GetBytes(val));
        protected void WriteUInt16(ushort val) => Write(Converter.GetBytes(val));
        protected void WriteUInt32(uint val) => Write(Converter.GetBytes(val));
        protected void WriteValueType(object obj, Type memberType) => Write(Converter.GetBytes(obj, memberType));
        protected virtual void WriteMember(object data, Type memberType)
        {
            if (memberType.IsArray)
            {
                WriteArray(data, memberType.GetType().GetElementType());
            }
            else
            {
                switch (memberType.Name)
                {
                    case "Nullable`1": WriteMember(data, memberType.GetGenericArguments()[0]); break;
                    case "BitArray": WriteBitArray((BitArray)data); break;
                    case "Bool": WriteBoolean((bool)(data ?? false)); break;
                    case "Byte" : WriteByte((byte)(data ?? (byte)0)); break;
                    case "ByteArray" : WriteByteArray((ByteArray)data); break;
                    case "Char" : WriteChar((char)(data ?? ' ')); break;
                    case "DateTime" : WriteDateTime((DateTime)data); break;
                    case "Double" : WriteDouble((double)(data ?? 0)); break;
                    case "Int16": WriteInt16((short)(data ?? (short)0)); break;
                    case "Int32" : WriteInt32((int)(data ?? 0)); break;
                    case "SByte" : WriteSByte((sbyte)(data ?? (sbyte)0)); break;
                    case "Single" : WriteSingle((float)(data ?? 0F)); break;
                    case "UInt16" : WriteUInt16((ushort)(data ?? (ushort)0)); break;
                    case "UInt32" : WriteUInt32((uint)(data ?? (uint)0)); break;
                    case "String" : WriteString((string)data); break;
                };
            }
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
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                Converter = null;
                TypeSurrogateSelector = null;
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~STDFBinaryFormatter()
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
