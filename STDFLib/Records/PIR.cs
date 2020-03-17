namespace STDFLib
{
    /// <summary>
    /// Part Information Record
    /// </summary>
    public class PIR : STDFRecord
    {
        protected override RecordType TypeCode => 0x0510;

        [STDF(Order = 1)]
        public byte HEAD_NUM { get; set; } = 0x01;

        [STDF(Order = 2)]
        public byte SITE_NUM { get; set; } = 0x01;

        public override string Description => "Part Information Record";
    }
}
