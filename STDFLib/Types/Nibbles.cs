using System;
using System.Collections.Generic;
using System.Text;

namespace STDFLib
{
    public class Nibbles
    {
        // The nibble array
        private readonly List<byte> nibbles;

        public static implicit operator Nibbles(byte[] buffer)
        {
            return new Nibbles(buffer);
        }

        public static implicit operator byte[](Nibbles fld)
        {
            return fld.GetNibbles();
        }

        // Constructor to allow expandable nibble array, up to ushort.MaxValue * 2 nibbles.
        public Nibbles()
        {
            nibbles = new List<byte>();
        }

        // Constructor to initialize nibble array with a packed array of nibbles 
        // Low 4 bits of byte 0 is nibble 1, upper 4 bits of byte 0 is nibble 2, etc.
        public Nibbles(byte[] buffer)
        {
            nibbles = new List<byte>(buffer);
            Count = buffer.Length * 2;
        }

        // Indexer to get / set the nibble at the given nibble index.  The nibble index must be within the range of the existing number of bytes in the array (not the total nibble capacity).        
        public byte this[int index]
        {
            get
            {
                if (index > (Count))
                {
                    throw new IndexOutOfRangeException("Nibble index out of range.");
                }

                return GetNibble(index);
            }

            set
            {
                if (index > (Count))
                {
                    throw new IndexOutOfRangeException("Nibble index out of range.");
                }

                SetNibble(value, index);
            }
        }

        // Return the number of nibbles currently in the nibble array.
        public int Count { get; private set; } = 0;

        // Return the byte count of the nibble array
        public int ByteCount
        {
            get
            {
                return Count == 0 ? 0 : Count / 2 + 1;
            }
        }

        public void Clear()
        {
            nibbles.Clear();
            Count = 0;
        }

        // Add a nibble to the end of the array.  If the index of the new nibble is an odd number, then the nibble is added into the 4 high order bits of the current last byte in the array.
        // If the new nibble index is even, then a new byte is added to the array with the nibble encoded in the lower 4 bits.
        // NOTE: it is assumed that the nibble is contained in the lower 4 bits of the pass nibble value.  The 4 higher order are ignored.
        public void Add(byte nibble)
        {
            if (Count % 2 == 0)
            {
                // next nibble will be have an even numbered index, so we just add the passed nibble as a new byte in the nibble array, with the higher order 4 bits zeroed out
                nibbles.Add((byte)(nibble & 0x0F));
            }
            else
            {
                // next nibble is an odd numbered index, meaning it needs to be added as the 4 higher ordered bits of the last byte in the nibble array
                nibbles[nibbles.Count - 1] = (byte)(nibbles[nibbles.Count - 1] & ((nibble << 4) | 0x0F));
            }

            // Increase the nibble count
            Count++;
        }

        // Adds a collection of nibbles to the nibble array.  Each byte is assumed to contain one nibble, with the nibble value contained in the 4 lower order bits.
        public void AddRange(byte[] nibbles)
        {
            foreach(var nibble in nibbles)
            {
                Add(nibble);
            }
        }

        // Convert the nibble array to an array of bytes.  
        // For odd numbered nibbles, the high 4 bits of the last byte will always be zero.  The number of bytes returned = (nibble_count / 2) + 1.
        public byte[] GetNibbles()
        {
            return nibbles.ToArray();
        }

        // Get the specified nibble
        protected byte GetNibble(int index)
        {
            if (index > Count || index < 0)
            {
                throw new IndexOutOfRangeException("Value for the zero-based nibble index is out of bounds (must be >= 0 and < number of nibbles)");
            }

            // Get the byte where the nibble is located
            byte nibbleByte = nibbles[index / 2];

            // To get the right nibble:
            // Calculate the byte index that the nibble is contained in:  byte index = nibble index/2
            // Then shift either 0 or 4 bits to the right to get the nibble into the lower 4 bits of a byte:  if index is odd, then shift 4 bits, if index is even, then no shift.
            // Then do a bitwise AND with 0x0F (bit pattern = '0000 1111') to make sure the 4 higher order bits are zero in the byte that is returned.
            return (byte)((nibbleByte >> (index % 2)*4) & 0x0F);
        }

        // Set the specified nibble.  NOTE: The nibble value is the 4 low order bits of the passed nibble byte.  The 4 high order bits are ignored.
        // EXAMPLE:
        //
        // Byte passed: 0101 0011  -> Nibble stored: 0011
        // Byte passed: 0110 0011  -> Nibble stored: 0011
        //
        // The 4 lower order bits are then shifted if needed and 
        protected void SetNibble(byte nibble, int index)
        {
            if ((index / 2 + 1) > Count || index < 0)
            {
                throw new IndexOutOfRangeException("Value for the zero-based nibble index is out of bounds (must be >= 0 and < number of nibbles)");
            }

            // Get the byte where the nibble will be stored
            byte nibbleByte = nibbles[index / 2];

            // Create a bit mask for the byte that will hold the new nibble.  If new nibble index is even, mask = 1111 0000,  if new nibble index is odd, mask = 0000 1111
            byte bitmask = (byte)((index % 2) == 0 ? (nibbleByte & 0xF0) : (nibbleByte & 0x0F));

            // OR the lower 4 bits of the new nibble into the bitmask.
            bitmask = (byte)(bitmask | (nibble & 0x0F));

            // Now, AND the bitmask with the existing byte that will hold the new nibble.  This will replace the appropriate 4 bit nibble within the byte with the new nibble value
            nibbles[index / 2] = (byte)(nibbleByte & bitmask);
        }

        // Convert the nibble at index nibbleIndex to a text representation of the bit values ( '1101' ) - mainly for easy testing
        public char[] ToCharArray(int nibbleIndex)
        {
            char[] ns = new char[4];

            byte nibble = GetNibble(nibbleIndex);

            for (int j = 3; j >= 0; j--)
            {
                ns[j] = (((nibble & 0x01) > 0) ? '1' : '0');
                nibble = (byte)(nibble >> 1);
            }

            return ns;
        }

        // Convert the nibble array to a string.  Each nibble representation is separated by a space.  For example:
        // Byte 0: 0x3F
        // Byte 1: 0xD2
        // Byte 3: 0x03
        //
        // Will produce a string representing the nibbles in index order (low order bits = first nibble, high order bits = second nibble, ...):
        // 
        // Nibble #:      1    2    3    4    5    6
        // Hex Value:     F    3    2    D    3    0
        // String:     1111 0011 0010 1101 0001 0000
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0;i < Count;i++)
            {
                sb.Append(ToCharArray(i));
                sb.Append(' ');
            }
            return sb.ToString();
        }
    }
}
