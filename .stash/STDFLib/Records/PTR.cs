namespace STDFLib2
{
    /// <summary>
    /// Parametric Test Record
    /// </summary>
    public class PTR : STDFRecord, ITestResult, IHasDefaults
    {
        public PTR() : base(RecordTypes.PTR, "Parametric Test Record") { }

        [STDF] public uint TEST_NUM { get; set; } = 0x00;
        [STDF] public byte HEAD_NUM { get; set; } = 0x00;
        [STDF] public byte SITE_NUM { get; set; } = 0x00;
        [STDF] public byte TEST_FLG { get; set; } = (byte)TestResultFlags.InvalidResult;
        [STDF] public byte PARM_FLG { get; set; } = 0x00;
        [STDF] public float? RESULT { get; set; } = 0;
        [STDF] public string TEST_TXT { get; set; } = "";
        [STDF] public string ALARM_ID { get; set; } = "";
        [STDFOptional] public byte OPT_FLAG { get; set; } = (byte)PTROptionalData.AllOptionalDataInvalid;
        [STDFOptional] public sbyte? RES_SCAL { get; set; } = null;
        [STDFOptional] public sbyte? LLM_SCAL { get; set; } = null;
        [STDFOptional] public sbyte? HLM_SCAL { get; set; } = null;
        [STDFOptional] public float? LO_LIMIT { get; set; } = null;
        [STDFOptional] public float? HI_LIMIT { get; set; } = null;
        [STDFOptional] public string UNITS { get; set; } = "";
        [STDFOptional] public string C_RESFMT { get; set; } = "";
        [STDFOptional] public string C_LLMFMT { get; set; } = "";
        [STDFOptional] public string C_HLMFMT { get; set; } = "";
        [STDFOptional] public float? LO_SPEC { get; set; } = null;
        [STDFOptional] public float? HI_SPEC { get; set; } = null;

        public override string ToString()
        {
            return string.Format("** Test# {0},{1},{2},{3}", TEST_NUM, HEAD_NUM, SITE_NUM, "Parametric");
        }
    }
}
