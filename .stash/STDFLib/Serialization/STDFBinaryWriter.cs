using STDFLib2.Serialization;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace STDFLib2
{
    public class STDFBinaryWriter : ISTDFBinaryWriter, IDisposable
    {
        protected bool LeaveOpen = false;
        protected Stream OutStream;
        
        public ISTDFFormatterConverter Converter { get; set; }
        public Stream BaseStream { get => OutStream; }

        public STDFBinaryWriter(Stream stream) : this(stream, new STDFFormatterConverter(), false) { }
        public STDFBinaryWriter(Stream stream, ISTDFFormatterConverter converter) : this(stream, converter, false) { }
        public STDFBinaryWriter(Stream stream, ISTDFFormatterConverter converter, bool leaveOpen)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if (!stream.CanWrite)
            {
                throw new InvalidOperationException("Stream passed to STDFBinaryWriter is not writeable.");
            }
            OutStream = stream;
            LeaveOpen = leaveOpen;
            Converter = converter;
        }

        public void Flush()
        {
            OutStream.Flush();
        }

        public void Write(Array values)
        {
            if (values.Length > 0)
            {
                Write(values, 0, values.Length);
            }
        }

        public void Write(Array values, int index, int count)
        {
            Type elementType = values.GetType().GetElementType();
            if (index+count <= values.Length)
            {
                for (int i = 0; i < count; i++)
                {
                    Write(elementType, values.GetValue(index+i));
                }
            }
        }

        public void Write(byte[] values)
        {
            BaseStream.Write(values, 0, values.Length);
        }

        public void Write(byte[] values, int index, int count)
        {
            BaseStream.Write(values, index, count);
        }

        public long Seek(int offset, SeekOrigin origin)
        {
            return OutStream.Seek(offset, origin);
        }

        public long Position { get => OutStream.Position; }

        public void Write(sbyte value)
        {
            OutStream.WriteByte((byte)value);
        }

        public void Write(bool value)
        {
            OutStream.WriteByte((byte)(value ? 1 : 0));
        }

        public void Write(byte value)
        {
            OutStream.WriteByte(value);
        }

        public void Write(DateTime value)
        {
            // write number of seconds between the date/time of the value and the unix epoch (1/1/1970 00:00:00)
            Write((uint)value.Subtract(DateTime.UnixEpoch).TotalSeconds);
        }

        public void Write(double value)
        {
            OutStream.Write(Converter.GetBytes(value));
        }

        public void Write(float value)
        {
            OutStream.Write(Converter.GetBytes(value));
        }

        public void Write(int value)
        {
            OutStream.Write(Converter.GetBytes(value));
        }

        public void Write(long value)
        {
            OutStream.Write(Converter.GetBytes(value));
        }

        public void Write(short value)
        {
            OutStream.Write(Converter.GetBytes(value));
        }

        public void Write(string value)
        {
            OutStream.Write(Converter.GetBytes(value));
        }

        public void Write(uint value)
        {
            OutStream.Write(Converter.GetBytes(value));
        }

        public void Write(ulong value)
        {
            OutStream.Write(Converter.GetBytes(value));
        }

        public void Write(ushort value)
        {
            OutStream.Write(Converter.GetBytes(value));
        }

        public void Write(Nibbles value)
        {
            OutStream.Write(Converter.GetBytes(value));
        }

        public void Write(BitField value)
        {
            OutStream.Write(Converter.GetBytes(value));
        }

        public void Write(BitField2 value)
        {
            OutStream.Write(Converter.GetBytes(value));
        }

        public void Write(Type dataType, object value)
        {
            // If nullable, get the underlying type name

            string typeName = dataType.Name == "Nullable`1" ? Nullable.GetUnderlyingType(dataType).Name : dataType.Name;

            // write the field
            switch (typeName)
            {
                case "Byte":
                    Write((byte)value);
                    break;
                case "SByte":
                    Write((sbyte)value);
                    break;
                case "Bool":
                    Write((bool)value);
                    break;
                case "Char":
                    if (Converter.Encoding is ASCIIEncoding)
                    {
                        Write((byte)(char)value);
                    } else
                    {
                        Write((char)value);
                    }
                    break;
                case "DateTime":
                    Write((DateTime)value);
                    break;
                case "Int16":
                    Write((short)value);
                    break;
                case "Int32":
                    Write((int)value);
                    break;
                case "UInt16":
                    Write((ushort)value);
                    break;
                case "UInt32":
                    Write((uint)value);
                    break;
                case "Single":
                    Write((float)value);
                    break;
                case "Double":
                    Write((double)value);
                    break;
                case "String":
                    Write((string)value);
                    break;
                case "Nibbles":
                    Write((Nibbles)value);
                    break;
                case "BitField":
                    Write((BitField)value);
                    break;
                case "BitField2":
                    Write((BitField2)value);
                    break;
                case "VarDataField":
                    Write((VarDataField)value);
                    break;
            }
        }

        public void Write(VarDataField value)
        {
            bool evenByte = (BaseStream.Position % 2) == 0 ? true : false;
            if (!evenByte)
            {
                // If we are on an even byte in the stream and we are writing one of the 
                // following types, then we need to add a padding byte as these need to start writing on an even byte
                // (writing the type code would leave the stream position on an odd byte)
                switch (value.Value.GetType().Name)
                {
                    case "Int16":
                    case "Int32":
                    case "UInt16":
                    case "UInt32":
                    case "Single":
                    case "Double":
                        // write a padding byte
                        Write((byte)0);
                        break;
                }
            }
            // write the field type code
            Write((byte)value.TypeCode);

            // write the data field
            Write(value.Value.GetType(), value.Value);
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

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~STDFBinaryWriter()
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
