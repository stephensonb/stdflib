namespace STDFLib
{
    public class STDFUInt64 : STDFType<ulong>
    {
        public static implicit operator byte[](STDFUInt64 fld)
        {
            return fld.ToByteArray();
        }

        public static implicit operator ulong(STDFUInt64 fld)
        {
            return fld.Value;
        }

        public static implicit operator STDFUInt64(ulong val)
        {
            return new STDFUInt64(val);
        }

        public static implicit operator STDFUInt64(byte[] buffer)
        {
            return new STDFUInt64(buffer);
        }

        public STDFUInt64(ulong value) : base(value) { }

        public STDFUInt64(byte[] buffer) : this(buffer, 0) { }

        public STDFUInt64(byte[] buffer, int start) : base(Converter.ToUInt64(buffer, start)) { }
    }
}
