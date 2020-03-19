using System;
using System.Collections.Generic;

namespace STDFLib
{
    /// <summary>
    /// Represents a variable size bit field and methods to perform operations on individual bits within the bit field.
    /// </summary>
    public class BitField 
    { 
        private List<byte> bit_array;

        private int max_bits;

        // store these in an array to speed things up
        private byte[] bit_mask = new byte[] 
        {
            0b00000001,  // bit 0 is 'on'
            0b00000010,  // bit 1 is 'on'
            0b00000100,  // bit 2 is 'on'
            0b00001000,  // bit 3 is 'on'
            0b00010000,  // bit 4 is 'on'
            0b00100000,  // bit 5 is 'on'
            0b01000000,  // bit 6 is 'on'
            0b10000000   // bit 7 is 'on'
        };

        // store these in an array to speed things up
        private byte[] bit_mask_complement = new byte[]
        {
            0b11111110,  // bit 0 is 'off'
            0b11111101,  // bit 1 is 'off'
            0b11111011,  // bit 2 is 'off'
            0b11110111,  // bit 3 is 'off'
            0b11101111,  // bit 4 is 'off'
            0b11011111,  // bit 5 is 'off'
            0b10111111,  // bit 6 is 'off'
            0b01111111   // bit 7 is 'off'
        };

        public BitFieldEncoding Encoding { get; }

        /// <summary>
        /// Variable length bit-encoded field.  Maximum number of bits is 8*255 = 2040 bits.
        /// </summary>
        public BitField() : this(BitFieldEncoding.PrefixedWithByteCount)
        {
            // Default to single byte length for the bit field, which means max of 8*255 bits = 2040.
        }

        // Constructor to specify the possible size of the bit field.  The number of bits that can be held is an unsigned integer = 2^(lengthBytes*8)
        protected BitField(BitFieldEncoding binaryEncoding)
        {
            bit_array = new List<byte>();

            Encoding = binaryEncoding;
                                 
            switch(binaryEncoding)
            {
                case BitFieldEncoding.PrefixedWithByteCount:
                    max_bits = 2040;
                    break;
                case BitFieldEncoding.PrefixedWithBitCount:
                    max_bits = 65536;
                    break;
            }
        }

        public ushort BitCount
        {
            get
            {
                return (ushort)(bit_array.Count * 8);
            }
        }

        public byte ByteCount
        {
            get
            {
                return (byte)(bit_array.Count);
            }
        }

        // Get the max bit count that this bit field can contain
        public virtual int MaxBitCount
        {
            get
            {
                return max_bits;
            }
        }
        
        // Set the bit array to a new set of bytes
        public void SetBits(byte[] bits)
        {
            bit_array.Clear();
            bit_array.AddRange(bits);
        }

        /// <summary>
        /// Bit number starts from the LSB of the first byte in the bit field and progresses toward the MSB at the end of the field.
        /// </summary>
        /// <param name="bit">Zero based index of the bit value to get (first bit in bit field is bit 0).</param>
        /// <returns>1 if bit is set, 0 if bit is not set, -1 if the bit number to get was invalid</returns>
        public virtual int GetBit(ushort bit)
        {
            int byteIndex = GetByteIndex(bit);
            int bitIndex = GetBitIndex(bit);

            if (byteIndex < 0)
            {
                return -1;
            }

            // We AND the corresponding bit mask value with the byte where the bit in question
            // is located.  IF the bit is set, then the result of this operation will be > 0
            // otherwise we return 0
            return (bit_array[byteIndex] & bit_mask[bitIndex]) > 0 ? 1 : 0;
        }

        // Set an individual bit to bitValue:  If bitValue <= 0, then set bit to 0, if bitValue > 0, then set the bit to 1
        public virtual void SetBit(ushort bit, int bitValue)
        {
            int byteIndex = GetByteIndex(bit);
            int bitIndex = GetBitIndex(bit);

            if (byteIndex >= 0)
            {
                // Grow the bit array if necessary
                GrowBitArray(byteIndex);

                // Set the bit value to 0
                if (bitValue <= 0)
                {
                    // To reset the bit, we AND the corresponding bit_mask_complement with the target bit set to zero, then AND it to clear the bit
                    bit_array[byteIndex] = (byte)(bit_array[byteIndex] & (bit_mask_complement[bitIndex]));
                }
                else
                {
                    // To set the bit, we OR the corresponding bit mask pattern to set it
                    bit_array[byteIndex] = (byte)(bit_array[byteIndex] | (bit_mask[bitIndex]));
                }
            }
        }

        // Sets the requested bit to 1 if bitValue is true, or zero if bitValue is false
        public virtual void SetBit(ushort bit, bool bitValue)
        {
            SetBit(bit, bitValue ? 1 : 0);
        }

        // Toggle a bit (set to 0 if it was 1 or set to 1 if it was 0)
        public virtual void ToggleBit(ushort bit)
        {
            int byteIndex = GetByteIndex(bit);
            int bitIndex = GetBitIndex(bit);

            if (byteIndex >= 0)
            {
                // To toggle the bit, we shift the bit we want to set to the right position, then XOR it to set it if original bit was 0, or reset it if original bit was 1.
                bit_array[byteIndex] = (byte)(bit_array[byteIndex] ^ bit_mask[bitIndex]);
            }
        }

        // Get the index of the byte that contains the bit we want to set/get
        private int GetByteIndex(ushort bit)
        {
            int index = -1;

            if (bit < max_bits)
            {
                index = (bit / 8); // Get the zero based index into the byte array;
            }

            return index;
        }

        // Grows the bit array from the current size to the size needed to be able to access a byte at byteIndex.
        private void GrowBitArray(int byteIndex)
        {
            // If the byte that contains the bit is past the end of the bit array, then expand the array up to the new byte index, 
            // filling with zeros along the way.
            if (byteIndex > (bit_array.Count - 1))
            {
                for (int i = 0; i < (byteIndex - bit_array.Count + 1); i++)
                {
                    bit_array.Add(0);
                }
            }
        }
        
        // Get the bit position within the byte that contains the bit number.  Bit index will be 0 through 7.  
        private int GetBitIndex(ushort bit)
        {
            // The bit number modulo 8 (the remainder) is the bit position within the byte that holds the bit.
            return (bit % 8);
        }

        // Return the bit array as an array of bytes.
        public byte[] GetBytes()
        {
            return bit_array.ToArray();
        }

        /// <summary>
        /// Returns the bit field as a character array of '1' and '0'.  The bits are returned as stored in the file per the spec.
        /// First data bit is the least significant bit of the first byte following the length bytes.
        /// </summary>
        /// <returns></returns>
        public virtual char[] ToCharArray()
        {
            char[] chars = new char[bit_array.Count * 8];

            for(int i=0;i<bit_array.Count * 8;i++)
            {
                chars[i] = (GetBit((ushort)i) == 1) ? '1' : '0';
            }
            return chars;
        }

        public override string ToString()
        {
            string strOut = new string(ToCharArray());
            if (strOut == "")
            {
                return "(*EMPTY)";
            }
            return strOut;
        }
    }
}
