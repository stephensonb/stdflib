namespace STDFLib
{
    public class STDFInt64 : STDFType<long>
    {
        public static implicit operator byte[](STDFInt64 fld)
        {
            return fld.ToByteArray();
        }

        public static implicit operator long(STDFInt64 fld)
        {
            return fld.Value;
        }

        public static implicit operator STDFInt64(long val)
        {
            return new STDFInt64(val);
        }

        public static implicit operator STDFInt64(byte[] buffer)
        {
            return new STDFInt64(buffer);
        }

        public STDFInt64(long value) : base(value) { }

        public STDFInt64(byte[] buffer) : this(buffer, 0) { }

        public STDFInt64(byte[] buffer, int start) : base(Converter.ToInt64(buffer, start)) { }
    }
}
