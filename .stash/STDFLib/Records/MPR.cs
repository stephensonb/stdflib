namespace STDFLib2
{
    /// <summary>
    /// Multi-result Parametric Record
    /// </summary>
    public class MPR : STDFRecord, ITestResult, IHasDefaults
    {
        public MPR() : base(RecordTypes.MPR, "Multiresult Parametric Record") { }

        [STDF] public uint TEST_NUM { get; set; } = 0x00;
        [STDF] public byte HEAD_NUM { get; set; } = 0x00;
        [STDF] public byte SITE_NUM { get; set; } = 0x00;
        [STDF] public byte TEST_FLG { get; set; } = (byte)TestResultFlags.InvalidResult;
        [STDF] public byte PARM_FLG { get; set; } = (byte)TestParameterFlags.AllBitsZero;
        [STDF] public ushort RTN_ICNT { get => (ushort)(RTN_STAT?.Length ?? 0); set { RTN_STAT = new byte[value]; RTN_INDX = new ushort[value]; } }
        [STDF] public ushort RSLT_CNT { get => (ushort)(RTN_RSLT?.Length ?? 0); set => RTN_RSLT = new float[value]; }
        [STDF] public byte[] RTN_STAT { get; set; } = new byte[0];
        [STDF] public float[] RTN_RSLT { get; set; } = new float[0];
        [STDF] public string TEST_TXT { get; set; } = "";
        [STDF] public string ALARM_ID { get; set; } = "";
        [STDFOptional] public byte OPT_FLAG { get; set; } = 0;
        [STDFOptional] public sbyte? RES_SCAL { get; set; } = null;
        [STDFOptional] public sbyte? LLM_SCAL { get; set; } = null;
        [STDFOptional] public sbyte? HLM_SCAL { get; set; } = null;
        [STDFOptional] public float? LO_LIMIT { get; set; } = null;
        [STDFOptional] public float? HI_LIMIT { get; set; } = null;
        [STDFOptional] public float? START_IN { get; set; } = null;
        [STDFOptional] public float? INCR_IN { get; set; } = null;
        [STDFOptional] public ushort[] RTN_INDX { get; set; } = new ushort[0];
        [STDFOptional] public string UNITS { get; set; } = "";
        [STDFOptional] public string UNITS_IN { get; set; } = "";
        [STDFOptional] public string C_RESFMT { get; set; } = "";
        [STDFOptional] public string C_LLMFMT { get; set; } = "";
        [STDFOptional] public string C_HLMFMT { get; set; } = "";
        [STDFOptional] public float? LO_SPEC { get; set; } = null;
        [STDFOptional] public float? HI_SPEC { get; set; } = null;

        public override string ToString()
        {
            return string.Format("** Test# {0},{1},{2},{3}", TEST_NUM, HEAD_NUM, SITE_NUM, "Multiresult Parametric");
        }
    }
}
