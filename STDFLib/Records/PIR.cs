namespace STDFLib
{
    /// <summary>
    /// Part Information Record
    /// </summary>
    public class PIR : STDFRecord
    {
        public PIR() : base(RecordTypes.PIR, "Part Information Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0x01;

        [STDF] public byte SITE_NUM { get; set; } = 0x01;
    }
}
