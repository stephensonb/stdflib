using System;

namespace STDFLib
{
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


}
