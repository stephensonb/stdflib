namespace STDFLib
{
    public class STDFByte : STDFType<byte>
    {
        public static implicit operator byte(STDFByte fld)
        {
            return fld.Value;
        }
        public static implicit operator STDFByte(byte val)
        {
            return new STDFByte(val);
        }
        public static implicit operator STDFByte(byte[] buffer)
        {
            return new STDFByte(buffer, 0);
        }

        public STDFByte(byte value) : base(value) { }

        public STDFByte(byte[] buffer) : base(buffer[0]) { }

        public STDFByte(byte[] buffer, int start) : base(buffer[start]) { }
    }
}
