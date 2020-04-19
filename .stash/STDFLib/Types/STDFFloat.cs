namespace STDFLib
{
    public class STDFFloat : STDFType<float>
    {
        public static implicit operator byte[](STDFFloat fld)
        {
            return fld.ToByteArray();
        }

        public static implicit operator float(STDFFloat fld)
        {
            return fld.Value;
        }

        public static implicit operator STDFFloat(float val)
        {
            return new STDFFloat(val);
        }

        public static implicit operator STDFFloat(byte[] buffer)
        {
            return new STDFFloat(buffer);
        }

        public STDFFloat(float value) : base(value) { }

        public STDFFloat(byte[] buffer) : this(buffer, 0) { }

        public STDFFloat(byte[] buffer, int start) : base(Converter.ToFloat(buffer, start)) { }
    }
}
