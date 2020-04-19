namespace STDFLib2
{
    /// <summary>
    /// Part Information Record
    /// </summary>
    public class PIR : STDFRecord
    {
        public PIR() : base(RecordTypes.PIR, "Part Information Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0x01;
        [STDF] public byte SITE_NUM { get; set; } = 0x01;

        public override string ToString()
        {
            return string.Format("** Part Info {0},{1}", HEAD_NUM, SITE_NUM);
        }
    }
}
