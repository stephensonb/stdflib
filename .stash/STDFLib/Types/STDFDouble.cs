namespace STDFLib
{
    public class STDFDouble : STDFType
    {
        public STDFDouble(object val) : base(val)
        {
        }

        public override int SerializedLength => sizeof(double);

        public override long Deserialize(byte[] buffer, long start)
        {
            Value = Converter.ToDouble(buffer, start);
            return start + 1;
        }

        public override byte[] Serialize()
        {
            return Converter.GetBytes(Value);
        }

        public static implicit operator double(STDFDouble fld)
        {
            return (double)fld.Value;
        }

        public static implicit operator STDFDouble(double val)
        {
            return new STDFDouble(val);
        }
    }
}
