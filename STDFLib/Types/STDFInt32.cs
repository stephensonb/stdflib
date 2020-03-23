namespace STDFLib
{
    public class STDFInt32 : STDFType<int>
    {
        public static implicit operator byte[](STDFInt32 fld)
        {
            return fld.ToByteArray();
        }

        public static implicit operator int(STDFInt32 fld)
        {
            return fld.Value;
        }

        public static implicit operator STDFInt32(int val)
        {
            return new STDFInt32(val);
        }

        public static implicit operator STDFInt32(byte[] buffer)
        {
            return new STDFInt32(buffer);
        }

        public STDFInt32(int value) : base(value) { }

        public STDFInt32(byte[] buffer) : this(buffer, 0) { }

        public STDFInt32(byte[] buffer, int start) : base(Converter.ToInt32(buffer, start)) { }
    }
}
