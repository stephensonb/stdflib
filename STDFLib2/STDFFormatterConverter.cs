using System;
using System.Text;

namespace STDFLib2
{
    public class STDFFormatterConverter : IFormatterConverter
    {
        public STDFFormatterConverter() : this(Encoding.ASCII) { }

        public STDFFormatterConverter(Encoding encoding)
        {
            SwapBytes = false;
            Encoding = encoding;
        }
        public bool SwapBytes { get; set; }
        private Endianness _endianness;
        public Encoding Encoding { get; }
        public Endianness Endianness
        {
            get => (SwapBytes && BitConverter.IsLittleEndian ? Endianness.BigEndian : Endianness.LittleEndian);

            private set
            {
                _endianness = value;
                // If the converter endianness is now different than the underlying architecture endiannes, 
                // set the SwapBytes flag to indicate any bytes read from a stream that are to be converted into 
                // a numeric value need to be swapped.
                if (_endianness != (BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian))
                {
                    SwapBytes = true;
                }
                else
                {
                    SwapBytes = false;
                }
            }
        }
        public object Convert(object value, Type toType)
        {
            if (toType.Name == "Nullable`1")
            {
                toType = toType.GetGenericArguments()[0];
            }

            return toType.Name switch
            {
                "Boolean" => ToBoolean(value),
                "Byte" => ToByte(value),
                "Char" => ToChar(value),
                "Double" => ToDouble(value),
                "Single" => ToSingle(value),
                "Int16" => ToInt16(value),
                "Int32" => ToInt32(value),
                "UInt16" => ToUInt16(value),
                "UInt32" => ToUInt32(value),
                "DateTime" => ToDateTime(value),
                "String" => ToString(value),
                "SByte" => ToSByte(value),
                "ByteArray" => ToByteArray(value),
                "BitArray" => ToBitArray(value),
                "NibbleArray" => ToNibbleArray(value),
                "Nibble" => ToNibble(value),
                _ => System.Convert.ChangeType(value, toType)
            };
        }
        public byte ToByte(object value)
        {
            if (value is byte[] buffer)
            {
                return buffer[0];
            }
            return System.Convert.ToByte(value);
        }
        public sbyte ToSByte(object value)
        {
            if (value is byte[] buffer)
            {
                return (sbyte)buffer[0];
            }
            return System.Convert.ToSByte(value);
        }
        public char ToChar(object value)
        {
            if (value is byte[] buffer)
            {
                return Encoding.GetChars(buffer)[0];
            }
            return System.Convert.ToChar(value);
        }
        public float ToSingle(object value)
        {
            if (value is byte[] buffer)
            {
                if (SwapBytes)
                {
                    ReverseBytes(buffer, sizeof(float));
                }
                return BitConverter.ToSingle(buffer);
            }
            return System.Convert.ToSingle(value);
        }
        public double ToDouble(object value)
        {
            if (value is byte[] buffer)
            {
                if (SwapBytes)
                {
                    ReverseBytes(buffer, sizeof(double));
                }
                return BitConverter.ToDouble(buffer);
            }

            // do default conversion
            return System.Convert.ToDouble(value);
        }
        public DateTime ToDateTime(object value)
        {
            if (value is byte[] buffer)
            {
                return DateTime.UnixEpoch.AddSeconds(ToUInt32(buffer));
            }
            return System.Convert.ToDateTime(value);
        }
        public string ToString(object value)
        {
            if (value != null)
            {
                if (value is byte[] buffer)
                {
                    // special handling for string length 1 with zero as the only char in the string.
                    // This represents NULL value internally, so convert to null.  Nulls will be converted back
                    // to same layout (length byte = 1, string byte = 0) when writing out to a stream.
                    if (buffer.Length == 1 && buffer[0] == 0)
                    {
                        return null;
                    }
                    return Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                }

                return System.Convert.ToString(value);
            }
            return null;
        }
        public short ToInt16(object value)
        {
            if (value is byte[] buffer)
            {
                if (SwapBytes)
                {
                    ReverseBytes(buffer, sizeof(short));
                }
                return BitConverter.ToInt16(buffer);
            }

            return System.Convert.ToInt16(value);
        }
        public ushort ToUInt16(object value)
        {
            if (value is byte[] buffer)
            {
                if (SwapBytes)
                {
                    ReverseBytes(buffer, sizeof(ushort));
                }
                return BitConverter.ToUInt16(buffer);
            }

            return System.Convert.ToUInt16(value);
        }
        public int ToInt32(object value)
        {
            if (value is byte[] buffer)
            {
                if (SwapBytes)
                {
                    ReverseBytes(buffer, sizeof(int));
                }
                return BitConverter.ToInt32(buffer);
            }
            return System.Convert.ToInt32(value);
        }
        public uint ToUInt32(object value)
        {
            if (value is byte[] buffer)
            {
                if (SwapBytes)
                {
                    ReverseBytes(buffer, sizeof(uint));
                }
                return BitConverter.ToUInt32(buffer);
            }

            return System.Convert.ToUInt32(value);
        }
        public bool ToBoolean(object value)
        {
            if (value is byte[] buffer)
            {
                return buffer[0] == 0 ? false : true;
            }

            return System.Convert.ToBoolean(value);
        }
        public Nibble ToNibble(object value)
        {
            return value as Nibble;
        }
        public NibbleArray ToNibbleArray(object value)
        {
            if (value == null || value is byte[] || value is NibbleArray)
            {
                return (NibbleArray)value;
            }
            throw new InvalidCastException(string.Format("Cannot cast type {0} to NibbleArray.", value.GetType().Name));
        }

        public ByteArray ToByteArray(object value)
        {
            if (value == null || value is byte[] || value is ByteArray)
            {
                return (ByteArray)value;
            }
            throw new InvalidCastException(string.Format("Cannot cast type {0} to ByteArray.", value.GetType().Name));
        }
        public BitArray ToBitArray(object value)
        {
            if (value == null || value is byte[] || value is BitArray)
            {
                return (BitArray)value;
            }
            throw new InvalidCastException(string.Format("Cannot cast type {0} to BitArray.", value.GetType().Name));
        }

        public byte[] GetBytes(object value)
        {
            if (value == null)
            {
                return null;
            }
            return GetBytes(value, value.GetType());
        }

        public byte[] GetBytes(object value, Type valueType)
        {
            byte[] barray;
            switch (valueType.Name)
            { 
                case "Bool": return new byte[] { (byte)((bool)value ? 1 : 0) };
                case "Byte": return new byte[] { (byte)value };
                case "Int16": return BitConverter.GetBytes((short)value);
                case "UInt16": return BitConverter.GetBytes((ushort)value);
                case "Int32": return BitConverter.GetBytes((int)value);
                case "UInt32": return BitConverter.GetBytes((uint)value);
                case "Single": return BitConverter.GetBytes((float)value);
                case "Double": return BitConverter.GetBytes((double)value);
                case "DateTime":
                    uint seconds = (uint)(((DateTime)value).Subtract(DateTime.UnixEpoch).TotalSeconds);
                    return BitConverter.GetBytes(seconds);
                case "String":
                    if (value == null)
                    {
                        // special null handling, represented with length byte = 1 and a single char
                        // value of zero as the string (empty string).
                        return new byte[1] { 0 };
                    }
                    int length = ((string)value).Length;
                    barray = new byte[length + 1];
                    barray[0] = (byte)length;
                    // copy the string chars to the buffer using ASCII (7-bit) encoding.
                    Encoding.ASCII.GetBytes((string)value).CopyTo(barray, 1);
                    return barray;
                case "Char":
                    return new byte[] { (byte)((char)value) };
                    //return new byte[] { (byte)System.Convert.ChangeType(value, typeof(byte)) };
                case "ByteArray":
                    ByteArray byteArray = (ByteArray)value;
                    barray = new byte[(byteArray?.ByteCount ?? 0) + 1];
                    if (barray.Length > 1)
                    { 
                        barray[0] = (byte)byteArray.ByteCount;
                        Array.Copy(byteArray, 0, barray, 1, byteArray.ByteCount);
                    }
                    return barray;
                case "BitArray":
                    BitArray bitArray = (BitArray)value;
                    barray = new byte[(bitArray?.ByteCount ?? 0) + 2];
                    if (barray.Length > 2)
                    {
                        Array.Copy(GetBytes((ushort)(bitArray.BitCount)), 0, barray, 0, 2);
                        Array.Copy(bitArray, 0, barray, 2, bitArray.ByteCount);
                    }
                    return barray;
                default: return new byte[] { };
            }
        }
        private void ReverseBytes(byte[] buffer, int length, int start = 0)
        {
            if (buffer.Length != 0)
            {
                byte[] rbuff = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    rbuff[i] = buffer[start + length - i - 1];
                }
                rbuff.CopyTo(buffer, start);
            }
        }
    }
}
