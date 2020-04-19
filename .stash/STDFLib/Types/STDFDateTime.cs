using System;

namespace STDFLib
{
    public class STDFDateTime : STDFType<DateTime>
    {
        public static implicit operator DateTime(STDFDateTime fld)
        {
            return fld.ToByteArray();
        }

        public static implicit operator uint(STDFDateTime fld)
        {
            return fld.Value;
        }

        public static implicit operator STDFDateTime(uint val)
        {
            return new STDFDateTime(val);
        }

        public static implicit operator STDFDateTime(byte[] buffer)
        {
            return new STDFDateTime(buffer);
        }

        public STDFDateTime(uint value) : base(value) { }

        public STDFDateTime(byte[] buffer) : this(buffer, 0) { }

        public STDFDateTime(byte[] buffer, int start) : base(Converter.ToUInt32(buffer, start)) { }
    }
}
