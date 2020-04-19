namespace STDFLib
{
    public class STDFUInt32 : STDFType<uint>
    {
        public static implicit operator byte[](STDFUInt32 fld)
        {
            return fld.ToByteArray();
        }

        public static implicit operator uint(STDFUInt32 fld)
        {
            return fld.Value;
        }

        public static implicit operator STDFUInt32(uint val)
        {
            return new STDFUInt32(val);
        }

        public static implicit operator STDFUInt32(byte[] buffer)
        {
            return new STDFUInt32(buffer);
        }

        public STDFUInt32(uint value) : base(value) { }

        public STDFUInt32(byte[] buffer) : this(buffer, 0) { }

        public STDFUInt32(byte[] buffer, int start) : base(Converter.ToUInt32(buffer, start)) { }
    }
}
