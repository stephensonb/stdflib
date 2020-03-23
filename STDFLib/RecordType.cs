namespace STDFLib
{
    public class RecordType
    {
        public ushort TypeCode;
        public byte REC_TYP { get => (byte)((TypeCode & 0xFF00) >> 8); }
        public byte REC_SUB { get => (byte)(TypeCode & 0x00FF); }

        public static implicit operator RecordType(ushort v)
        {
            return new RecordType() { TypeCode = v };
        }

        public static implicit operator RecordType(RecordTypes type)
        {
            return new RecordType { TypeCode = (ushort)type };
        }
    }


}
