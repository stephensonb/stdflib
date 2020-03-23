namespace STDFLib
{
    /// <summary>
    /// Begin Program Segment record
    /// </summary>
    public class BPS : STDFRecord
    {
        public BPS() : base(RecordTypes.BPS, "Begin Program Section") { }

        [STDF] public string SEQ_NAME { get; set; } = "";
    }
}
