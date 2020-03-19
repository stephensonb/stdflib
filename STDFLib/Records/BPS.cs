namespace STDFLib
{
    /// <summary>
    /// Begin Program Segment record
    /// </summary>
    public class BPS : STDFRecord
    {
        public override RecordType TypeCode => 0x140A;

        [STDF(Order = 1)]
        public string SEQ_NAME { get; set; } = "";

        public override string Description => "Begin Program Section";
    }
}
