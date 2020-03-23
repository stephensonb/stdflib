namespace STDFLib
{
    public class STDFBool : STDFType
    {
        public STDFBool(object val) : base(val)
        {
        }

        public override int SerializedLength => 1;

        public override long Deserialize(byte[] buffer, long start)
        {
            Value = buffer[start] == 0 ? false : true;
            return start + 1;
        }

        public override byte[] Serialize()
        {
            return new byte[] { (byte)((bool)Value ? 1 : 0) };
        }

        public static implicit operator bool(STDFBool fld)
        {
            return (bool)fld.Value;
        }

        public static implicit operator STDFBool(bool val)
        {
            return new STDFBool(val);
        }
    }
}
