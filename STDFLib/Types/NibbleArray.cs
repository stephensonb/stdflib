using System;

namespace STDFLib
{
    /// <summary>
    /// Class for inspecting and manipulating an array of Nibbles (4 bit data structure).
    /// </summary>
    public class NibbleArray
    {
        private byte[] Value { get; set; }
        private readonly byte[] _bitMask = new byte[]
        {
            0b11110000,
            0b00001111,
        };

        private NibbleArray(byte[] value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates an array of nibbles.
        /// </summary>
        /// <param name="nibbleCount">Numer of nibbles that the array will hold.</param>
        public NibbleArray(ushort nibbleCount)
        {
            if (nibbleCount == 0)
            {
                Value = new byte[0];
            }
            else
            {
                Value = new byte[(nibbleCount - 1) / 2 + 1];
            }
        }

        public static explicit operator byte[](NibbleArray value)
        {
            return value?.Value;
        }

        public static explicit operator NibbleArray(byte[] value)
        {
            return value != null ? new NibbleArray(value) : null;
        }

        /// <summary>
        /// Returns the size of the nibble array in bytes (will be the nibble count * 2).
        /// </summary>
        public int ByteCount => Value?.Length ?? 0;

        /// <summary>
        /// Returns the size of the nibble array in nibbles.
        /// </summary>
        public int NibbleCount => Value?.Length * 2 ?? 0;

        /// <summary>
        /// Override of Object.Equals.  Compares the current nibble array to "obj" for equality.
        /// </summary>
        /// <param name="obj">NibbleArray object to compare to.  Does a byte by byte comparison of the arrays for equality.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is NibbleArray array2)
            {
                if (Value.Length == array2.Value.Length)
                {
                    for (int i = 0; i < Value.Length; i++)
                    {
                        if (Value[i] != array2.Value[i])
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

        /// <summary>
        /// Indexer into the NibbleArray
        /// </summary>
        /// <param name="index">Index of the nibble to get/set.</param>
        /// <remarks>The value of the nibble is contained in the low 4 bits of the value passed.  All other bits are ignored.</remarks>
        /// <returns></returns>
        public byte this[int index]
        {
            get
            {
                if (index < 1 || index > NibbleCount)
                {
                    throw new IndexOutOfRangeException("Index out of range.");
                }

                // mask off the high bits for even nibble or low bits for odd nibble;
                byte rval = (byte)(Value[(index - 1) / 2] & _bitMask[index % 2]);

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
                    Value[(index - 1) / 2] |= (byte)((value & 0x0F) << 4);
                }
                else
                {
                    // odd nibbles are stored in low 4 bits, mask of the lower 4 bits of the byte containing the target nibble,
                    // then OR the new value with the target byte
                    Value[(index - 1) / 2] = (byte)(Value[(index - 1) / 2] & 0xF0 | value);
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
