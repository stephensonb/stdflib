namespace STDFLib2
{
    /// <summary>
    /// Part Information Record
    /// </summary>
    public class PIR : STDFRecord
    {
        public PIR() : base((ushort)RecordTypes.PIR) { }

        [STDF] public byte HEAD_NUM;
        [STDF] public byte SITE_NUM;

        public override string ToString()
        {
            return string.Format("** Part Info {0},{1}", HEAD_NUM, SITE_NUM);
        }
    }
}
