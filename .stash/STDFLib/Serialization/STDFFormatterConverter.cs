using System;
using System.Text;

namespace STDFLib2.Serialization
{
    public class STDFFormatterConverter : ISTDFFormatterConverter
    {
        public bool SwapBytes { get; set; }

        private Endianness _endianness;
        public Encoding Encoding { get; }

        public STDFFormatterConverter() : this(Encoding.ASCII) { }

        public STDFFormatterConverter(Encoding encoding)
        {
            SwapBytes = false;
            Encoding = encoding;
        }

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
            if (value is byte[] buffer)
            {
                // length is first byte
                int length = buffer[0];
                return Encoding.ASCII.GetString(buffer, 1, length);
            }

            return System.Convert.ToString(value);
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

        public byte[] GetBytes(object value)
        {
            byte[] barray;

            switch (value)
            { 
                case bool v: return BitConverter.GetBytes(v);
                case byte v: return BitConverter.GetBytes(v);
                case sbyte v: return BitConverter.GetBytes(v);
                case short v: return BitConverter.GetBytes(v);
                case ushort v: return BitConverter.GetBytes(v);
                case int v: return BitConverter.GetBytes(v);
                case uint v: return BitConverter.GetBytes(v);
                case float v: return BitConverter.GetBytes(v);
                case double v: return BitConverter.GetBytes(v);
                case DateTime v:
                    uint seconds = (uint)(v.Subtract(DateTime.UnixEpoch).TotalSeconds);
                    return BitConverter.GetBytes(seconds);
                case string v:
                    int length = v.Length;
                    barray = new byte[v.Length + 1];
                    barray[0] = (byte)length;
                    // copy the string chars to the buffer using ASCII (7-bit) encoding.
                    Encoding.ASCII.GetBytes(v).CopyTo(barray, 1);
                    return barray;
                case char _:
                    return new byte[] { (byte)value };
                default: return new byte[] { };
            }
        }

        private void ReverseBytes(byte[] buffer, int length, int start = 0)
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
