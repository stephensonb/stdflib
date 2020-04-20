namespace STDFLib
{
    /// <summary>
    /// Type representing a nibble - 4 bit chunk of data.
    /// </summary>
    public class Nibble
    {
        private byte Value { get; set; }
        
        private Nibble(byte value)
        {
            Value = value;
        }

        public static explicit operator byte(Nibble value)
        {
            return value != null ? value.Value : (byte)0;
        }

        public static explicit operator Nibble(byte value)
        {
            return new Nibble(value);
        }

        public static explicit operator short(Nibble value)
        {
            return value != null ? value.Value : (short)0;
        }

        public static explicit operator Nibble(short value)
        {
            return new Nibble((byte)value);
        }

        public static explicit operator int(Nibble value)
        {
            return value != null ? value.Value : 0;
        }

        public static explicit operator Nibble(int value)
        {
            return new Nibble((byte)value);
        }
        public static explicit operator ushort(Nibble value)
        {
            return value != null ? value.Value : (ushort)0;
        }

        public static explicit operator Nibble(ushort value)
        {
            return new Nibble((byte)value);
        }

        public static explicit operator uint(Nibble value)
        {
            return value != null ? value.Value : (uint)0;
        }

        public static explicit operator Nibble(uint value)
        {
            return new Nibble((byte)value);
        }
    }
}
