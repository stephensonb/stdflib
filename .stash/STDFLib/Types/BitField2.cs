namespace STDFLib2
{
    public class BitField2 : BitField
    {
        public static explicit operator BitField2(byte[] buffer)
        {
            return new BitField2(buffer);
        }

        public static explicit operator byte[](BitField2 fld)
        {
            return fld.GetBits();
        }

        /// <summary>
        /// Variable length bit-encoded field (max 65,535 bits)
        /// </summary>
        /// <param name="bits">Byte array containing the bit field.</param>
        /// <param name="length">Length of the bit field in bits</param>
        public BitField2() : base(ushort.MaxValue)
        {
        }

        public BitField2(byte[] buffer) : this()
        {
            SetBits(buffer);
        }
    }
}
