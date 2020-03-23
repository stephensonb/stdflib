namespace STDFLib
{
    public class BitField2 : BitField
    {
        /// <summary>
        /// Variable length bit-encoded field (max 65,535 bits)
        /// </summary>
        /// <param name="bits">Byte array containing the bit field.</param>
        /// <param name="length">Length of the bit field in bits</param>
        public BitField2() : base(BitFieldEncoding.PrefixedWithBitCount)
        {
        }

        public BitField2(byte[] buffer) : base(BitFieldEncoding.PrefixedWithBitCount)
        {
            SetBits(buffer);
        }
    }
}
