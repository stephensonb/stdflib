namespace STDFLib
{
    public class STDFInt16 : STDFType<short>
    {
        public static implicit operator byte[](STDFInt16 fld)
        {
            return fld.ToByteArray();
        }

        public static implicit operator short(STDFInt16 fld)
        {
            return fld.Value;
        }

        public static implicit operator STDFInt16(short val)
        {
            return new STDFInt16(val);
        }

        public static implicit operator STDFInt16(byte[] buffer)
        {
            return new STDFInt16(buffer);
        }

        public STDFInt16(short value) : base(value) { }

        public STDFInt16(byte[] buffer) : this(buffer,0) { }

        public STDFInt16(byte[] buffer, int start) : base(Converter.ToInt16(buffer,start)) { }
    }
}
