namespace STDFLib2
{
    public class Nibble
    {
        private byte _value { get; set; }
        private Nibble(byte value) 
        {
            _value = value;
        }

        public static implicit operator byte(Nibble value)
        {
            return value != null ? value._value : (byte)0;
        }

        public static implicit operator byte?(Nibble value)
        {
            return value != null ? value._value : (byte?)null;
        }

        public static implicit operator Nibble(byte? value)
        {
            return value != null ? new Nibble((byte)value) : null;
        }

        public static implicit operator Nibble(byte value)
        {
            return new Nibble(value);
        }
    }
}
