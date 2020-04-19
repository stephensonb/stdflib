using System;

namespace STDFLib2
{
    public class BitArray
    {
        private byte[] _value { get; set; }
        private byte[] _bitMask = new byte[]
        {
            0b00000001,
            0b00000010,
            0b00000100,
            0b00001000,
            0b00010000,
            0b00100000,
            0b01000000,
            0b10000000
        };

        private BitArray(byte[] value)
        {
            _value = value;
        }

        public BitArray(ushort bitCount)
        {
            if (bitCount == 0)
            {
                _value = new byte[0];
            }
            else
            {
                _value = new byte[(bitCount - 1) / 8 + 1];
            }
        }

        public static implicit operator byte[](BitArray value)
        {
            return value != null ? value._value : null;
        }

        public static implicit operator BitArray(byte[] value)
        {
            return value != null ? new BitArray(value) : null;
        }

        // size of bitarray in bytes
        public int ByteCount { get => _value?.Length ?? 0; }

        // size of bitarray in bits
        public int BitCount { get => _value?.Length * 8 ?? 0; }

        public void SetBitValue(int index, int state)
        {
            this[index] = state == 0;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is BitArray array2)
            {
                if (ByteCount != array2.ByteCount )
                {
                    return false;
                }

                for(int i = 0; i < ByteCount; i++)
                {
                    if (_value[i] != array2._value[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int GetBitValue(int index)
        {
            return this[index] ? 1 : 0;
        }

        // get / set individual bit by index
        public bool this[int index]
        {
            get
            {
                if (index < 1 || index > BitCount + 1)
                {
                    throw new IndexOutOfRangeException("Index out of range.");
                }

                index--;

                return (byte)(_value[index / 8] & _bitMask[index % 8]) > 0;
            }

            set
            {
                if (index < 1 || index > BitCount)
                {
                    throw new IndexOutOfRangeException("Index out of range.");
                }

                index--;

                if (value)
                {
                    // set bit to zero
                    _value[index / 8] ^= _bitMask[index % 8];
                }
                else
                {
                    // set bit to 1
                    _value[index / 8] |= _bitMask[index % 8];
                }
            }
        }

        public override string ToString()
        {
            int length = BitCount + BitCount / 8;
            char[] chars = new char[length];
            int strIndex = 0;
            for (int i = 1; i <= BitCount; i++)
            {
                if (i > 1 && i <= BitCount && (i - 1) % 8 == 0)
                {
                    chars[strIndex++] = ' ';
                }
                chars[strIndex++] = this[i] ? '1' : '0';
            }
            return new string(chars);
        }
    }
}
