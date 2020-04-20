using System;

namespace STDFLib
{
    /// <summary>
    /// Utility class to inspect and manipulate individual bits within an array of bytes.
    /// </summary>
    public class BitArray
    {
        private byte[] Value { get; set; }
        private readonly byte[] BitMask = new byte[]
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

        /// <summary>
        /// Creates a new bit array from an array of bytes
        /// </summary>
        /// <param name="value">Array of bytes to initialize the bit array with.</param>
        private BitArray(byte[] value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a new bit array
        /// </summary>
        /// <param name="bitCount">Number of bits the array should hold.</param>
        public BitArray(ushort bitCount)
        {
            if (bitCount == 0)
            {
                Value = new byte[0];
            }
            else
            {
                Value = new byte[(bitCount - 1) / 8 + 1];
            }
        }

        public static implicit operator byte[](BitArray value)
        {
            return value != null ? value.Value : null;
        }

        public static implicit operator BitArray(byte[] value)
        {
            return value != null ? new BitArray(value) : null;
        }

        /// <summary>
        /// Returns the size of the bit array in bytes
        /// </summary>
        public int ByteCount => Value?.Length ?? 0;


        /// <summary>
        /// Returns the number of bits held within the bit array
        /// </summary>
        public int BitCount => Value?.Length * 8 ?? 0;

        /// <summary>
        /// Sets a bit in the bit array
        /// </summary>
        /// <param name="index">The index of the bit to set.  Bit 1 is the high bit of the first byte in the array.</param>
        /// <param name="state">The state to set the bit to.  Zero clears the bit and any other non-zero number sets the bit to 1.</param>
        public void SetBitValue(int index, int state)
        {
            this[index] = state;
        }

        /// <summary>
        /// Override of Object.Equals.  Does a byte-by-byte comparison to determine if two bit arrays are equal.
        /// </summary>
        /// <param name="obj">The BitArray object to compare to.</param>
        /// <returns>
        /// True - BitArray contents are equal
        /// False - BitArray contents are not equal
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is BitArray array2)
            {
                if (ByteCount != array2.ByteCount)
                {
                    return false;
                }

                for (int i = 0; i < ByteCount; i++)
                {
                    if (Value[i] != array2.Value[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        // required since we override Equals.
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns the value of a bit (0 or 1).
        /// </summary>
        /// <param name="index">The index of the bit number to return.  Bit indexes start at 1.</param>
        /// <returns>
        /// State of the bit at index <b>index</b>.  Either 0 (not-set) or 1 (set);
        /// </returns>
        public int GetBitValue(int index)
        {
            return this[index];
        }

        /// <summary>
        /// Indexer into the bit array
        /// </summary>
        /// <param name="index">Index of the bit to get or set.</param>
        /// <returns>
        /// Get - Returns the current state of the bit at index <b>index</b>.
        /// Set - Sets the bit at index <b>index</b> to a new state.  Valid values are 0 to set the bit to zero, or any non-zero number to set to 1.
        /// </returns>
        public int this[int index]
        {
            get
            {
                if (index < 1 || index > BitCount + 1)
                {
                    throw new IndexOutOfRangeException("Index out of range.");
                }

                index--;

                return (byte)((Value[index / 8] & BitMask[index % 8]) > 0 ? 1 : 0);
            }

            set
            {
                if (index < 1 || index > BitCount)
                {
                    throw new IndexOutOfRangeException("Index out of range.");
                }

                index--;

                if (value == 0)
                {
                    // set bit to zero
                    Value[index / 8] ^= BitMask[index % 8];
                }
                else
                {
                    // set bit to 1
                    Value[index / 8] |= BitMask[index % 8];
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
                chars[strIndex++] = this[i] == 0 ? '0' : '1';
            }
            return new string(chars);
        }
    }
}
