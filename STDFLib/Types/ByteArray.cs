using System;

namespace STDFLib
{
    /// <summary>
    /// Class for inspecting and maniuplating an array of bytes
    /// </summary>
    public class ByteArray
    {
        private byte[] Value { get; set; }

        private ByteArray(byte[] value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a new byte array
        /// </summary>
        /// <param name="count">Size of the array in bytes</param>
        public ByteArray(int count)
        {
            Value = new byte[count];
        }

        public static implicit operator byte[](ByteArray value)
        {
            return value?.Value;
        }

        public static implicit operator ByteArray(byte[] value)
        {
            return value != null ? new ByteArray(value) : null;
        }

        /// <summary>
        /// Returns the number of bytes held within the byte array
        /// </summary>
        public int ByteCount => Value?.Length ?? 0;

        /// <summary>
        /// Determines if two byte arrays are equal
        /// </summary>
        /// <param name="obj">ByteArray object to compare to.  Does a byte by byte comparison to determin equality.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is ByteArray array2)
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Indexer into the byte array
        /// </summary>
        /// <param name="index">Zero based index of the byte to return</param>
        /// <returns></returns>
        public byte this[int index]
        {

            get
            {
                if (index < 0 || index > Value.Length)
                {
                    throw new IndexOutOfRangeException("Index out of range.");
                }
                return Value[index];
            }

            set
            {
                if (index < 0 || index > Value.Length)
                {
                    throw new IndexOutOfRangeException("Index out of range.");
                }
                Value[index] = value;
            }
        }
    }
}
