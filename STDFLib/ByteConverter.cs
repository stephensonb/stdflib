using System;
using System.Text;

namespace STDFLib
{
    public class ByteConverter
    {
        private bool SwapBytes = false;

        public Endianness Endianness { get; private set; }

        public ByteConverter()
        {
            Endianness = BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;
        }

        public void SetEndianness(Endianness endianness)
        {
            Endianness = endianness;
            // If the converter endianness is now different than the underlying architecture endiannes, 
            // set the SwapBytes flag to indicate any bytes read from a stream that are to be converted into 
            // a numeric value need to be swapped.
            if (endianness != (BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian))
            {
                SwapBytes = true;
            } else
            {
                SwapBytes = false;
            }
        }

        public float ToFloat(byte[] buffer, int start=0)
        {
            if(SwapBytes)
            {
                ReverseBytes(buffer, 4,start);
            }
            return BitConverter.ToSingle(buffer, start);
        }

        public double ToDouble(byte[] buffer, int start=0)
        {
            if(SwapBytes)
            {
                ReverseBytes(buffer, 8, start);
            }
            return BitConverter.ToDouble(buffer, start);
        }

        public Nibbles ToNibbles(byte[] buffer)
        {
            return new Nibbles(buffer);
        }

        public BitField ToBitField(byte[] buffer)
        {
            return new BitField(buffer);
        }

        public BitField2 ToBitField2(byte[] buffer)
        {
            return new BitField2(buffer);
        }

        public DateTime ToDateTime(byte[]  buffer, int start=0)
        {
            return DateTime.UnixEpoch.AddSeconds(ToUInt32(buffer,start));
        }

        public string ToString(byte[] buffer, int start=0)
        {
            return new string(ToCharArray(buffer, buffer.Length, start));
        }

        public char[] ToCharArray(byte[] buffer, int length, long start=0)
        {
            if(length == 0)
            {
                return new char[] { };
            }
            char[] carray = new char[length];
            for (long i = start; (i-start) < length && i < buffer.Length; i++)
            {
                carray[i] = (char)buffer[i];
            }
            return carray;
        }

        public short ToInt16(byte[] buffer, int start=0)
        {
            if(SwapBytes)
            {
                ReverseBytes(buffer, sizeof(short), start);
            }
            return BitConverter.ToInt16(buffer, start);
        }

        public ushort ToUInt16(byte[] buffer, int start = 0)
        {
            if (SwapBytes)
            {
                ReverseBytes(buffer, sizeof(ushort), start);
            }
            return BitConverter.ToUInt16(buffer, start);
        }
        public int ToInt32(byte[] buffer, int start = 0)
        {
            if (SwapBytes)
            {
                ReverseBytes(buffer, sizeof(int), start);
            }
            return BitConverter.ToInt32(buffer, start);
        }
        public uint ToUInt32(byte[] buffer, int start = 0)
        {
            if (SwapBytes)
            {
                ReverseBytes(buffer, sizeof(uint), start);
            }
            return BitConverter.ToUInt32(buffer, start);
        }
        public long ToInt64(byte[] buffer, int start = 0)
        {
            if (SwapBytes)
            {
                ReverseBytes(buffer, sizeof(long), start);
            }
            return BitConverter.ToInt64(buffer, start);
        }
        public ulong ToUInt64(byte[] buffer, int start = 0)
        {
            if (SwapBytes)
            {
                ReverseBytes(buffer, sizeof(ulong), start);
            }
            return BitConverter.ToUInt64(buffer, start);
        }
        public bool ToBool(byte[] buffer, int start = 0) => buffer[start] == 0 ? false : true;

        public byte[] GetBytes(object value)
        {
            byte[] barray;

            switch(value.GetType().Name)
            {
                case "Bool": return BitConverter.GetBytes((bool)value);
                case "Byte": return BitConverter.GetBytes((byte)value);
                case "SByte": return BitConverter.GetBytes((sbyte)value);
                case "Int16": return BitConverter.GetBytes((short)value);
                case "UInt16": return BitConverter.GetBytes((ushort)value);
                case "Int32": return BitConverter.GetBytes((int)value);
                case "UInt32": return BitConverter.GetBytes((uint)value);
                case "Int64": return BitConverter.GetBytes((long)value);
                case "UInt64": return BitConverter.GetBytes((ulong)value);
                case "Single": return BitConverter.GetBytes((float)value);
                case "Double": return BitConverter.GetBytes((double)value);
                case "String":
                    int length = ((string)value).Length + 1;
                    barray = new byte[((string)value).Length+1];
                    barray[0] = (byte)length;
                    if(length > 0)
                    {
                        // copy the string chars to the buffer using ASCII (7-bit) encoding.
                        ASCIIEncoding.ASCII.GetBytes((string)value).CopyTo(barray,1);
                    }
                    return barray;
                case "Char[]":
                    barray = new byte[((string)value).Length];
                    ASCIIEncoding.ASCII.GetBytes((char[])value).CopyTo(barray, 1);
                    return barray;
                case "Nibbles":
                    return ((Nibbles)value).GetNibbles();
                case "BitField":
                case "BitField2":
                    // Return the bit array as an array of bytes.
                    int prefixLength = (int)((BitField)value).Encoding;
                    byte[] bits = ((BitField)value).GetBits();
                    barray = new byte[bits.Length + prefixLength];
                    // Encode the length
                    if (prefixLength == 1)
                    {
                        barray[0] = (byte)bits.Length;
                    }
                    else
                    {
                        GetBytes((ushort)((BitField)value).BitCount).CopyTo(barray, 0);
                    }
                    // copy the bitfield to the buffer after the length byte(s)
                    bits.CopyTo(barray, prefixLength);
                    return barray;
                default: return new byte[] { };
            }
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

    }
}
