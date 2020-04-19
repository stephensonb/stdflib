using System;

namespace STDFLib2
{
    public class NibbleArray
    {
        private byte[] _value { get; set; }
        private byte[] _bitMask = new byte[]
        {
            0b11110000,
            0b00001111,
        };

        private NibbleArray(byte[] value)
        {
            _value = value;
        }

        public NibbleArray(ushort nibbleCount)
        {
            if (nibbleCount == 0)
            {
                _value = new byte[0];
            }
            else
            {
                _value = new byte[(nibbleCount - 1) / 2 + 1];
            }
        }

        public static implicit operator byte[](NibbleArray value)
        {
            return value != null ? value._value : null;
        }

        public static implicit operator NibbleArray(byte[] value)
        {
            return value != null ? new NibbleArray(value) : null;
        }

        // size of bitarray in bytes
        public int ByteCount { get => _value?.Length ?? 0; }

        // size of bitarray in bits
        public int NibbleCount { get => _value?.Length * 2 ?? 0; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is NibbleArray array2)
            {
                if (_value.Length == array2._value.Length)
                {
                    for (int i = 0; i < _value.Length; i++)
                    {
                        if (_value[i] != array2._value[i])
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // get / set individual bit by index
        public byte this[int index]
        {
            get
            {
                if (index < 1 || index > NibbleCount)
                {
                    throw new IndexOutOfRangeException("Index out of range.");
                }

                // mask off the high bits for even nibble or low bits for odd nibble;
                byte rval = (byte)(_value[(index - 1) / 2] & _bitMask[index % 2]);

                if (index % 2 == 0)
                {
                    // even nibbles are in high 4 bits, shift right to move them to low 4 bits for return
                    return (byte)(rval >> 4);
                }

                // odd nibbles are in low 4 bits, so no shift needed.
                return rval;
            }

            set
            {
                if (index < 1 || index > NibbleCount + 1)
                {
                    throw new IndexOutOfRangeException("Index out of range.");
                }

                if (value > 15)
                {
                    throw new Exception("Nibble value out of range.  Maximum value for a nibble is 15.");
                }

                if (index % 2 == 0)
                {
                    // even nibbles are stored in high 4 bits, mask off the high 4 bits of the new value,
                    // shift the new value 4 bits left and then OR them with the byte containing the nibble
                    _value[(index - 1) / 2] |= (byte)((value & 0x0F) << 4);
                }
                else
                {
                    // odd nibbles are stored in low 4 bits, mask of the lower 4 bits of the byte containing the target nibble,
                    // then OR the new value with the target byte
                    _value[(index - 1) / 2] = (byte)(_value[(index - 1) / 2] & 0xF0 | value);
                }
            }
        }

        public override string ToString()
        {
            string[] s = new string[NibbleCount];

            for (int i = 1; i <= NibbleCount; i++)
            {
                byte nibble = this[i];
                s[i - 1] = ((nibble & 0b1000) > 0 ? "1" : "0") +
                    ((nibble & 0b0100) > 0 ? "1" : "0") +
                    ((nibble & 0b0010) > 0 ? "1" : "0") +
                    ((nibble & 0b0001) > 0 ? "1" : "0") + " ";
            }
            return string.Join("", s);
        }
    }
}
