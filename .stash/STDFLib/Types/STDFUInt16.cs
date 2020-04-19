namespace STDFLib
{
    public class STDFUInt16 : STDFType
    {
        public STDFUInt16(object val) : base(val)
        {
        }

        public override int SerializedLength => sizeof(ushort);

        public override long Deserialize(byte[] buffer, long start)
        {
            Value = Converter.ToUInt16(buffer, start);
            return start + sizeof(ushort);
        }
        
        public override byte[] Serialize()
        {
            return Converter.GetBytes(Value);
        }

        public static implicit operator ushort(STDFUInt16 fld)
        {
            return (ushort)fld.Value;
        }

        public static implicit operator STDFUInt16(ushort val)
        {
            return new STDFUInt16(val);
        }
    }
}
