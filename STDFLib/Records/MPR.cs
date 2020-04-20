namespace STDFLib
{
    /// <summary>
    /// Multi-result Parametric Record
    /// </summary>
    public class MPR : STDFRecord
    {
        public MPR() : base(RecordTypes.MPR) { }

        [STDF] public uint TEST_NUM = 0;
        [STDF] public byte HEAD_NUM = 1;
        [STDF] public byte SITE_NUM = 1;
        [STDF] public byte TEST_FLG = (byte)TestResultFlags.InvalidResult;
        [STDF] public byte PARM_FLG = (byte)TestParameterFlags.AllBitsZero;
        [STDF] public ushort RTN_ICNT = 0;
        [STDF] public ushort RSLT_CNT = 0;
        [STDF("RTN_ICNT")] public byte[] RTN_STAT;
        [STDF("RSLT_CNT")] public float[] RTN_RSLT;
        [STDF] public string TEST_TXT = "";
        [STDF] public string ALARM_ID = "";
        [STDFOptional] public byte OPT_FLAG = 0;
        [STDFOptional] public sbyte? RES_SCAL;
        [STDFOptional] public sbyte? LLM_SCAL;
        [STDFOptional] public sbyte? HLM_SCAL;
        [STDFOptional] public float? LO_LIMIT;
        [STDFOptional] public float? HI_LIMIT;
        [STDFOptional] public float? START_IN;
        [STDFOptional] public float? INCR_IN;
        [STDFOptional("RTN_ICNT")] public ushort[] RTN_INDX;
        [STDFOptional] public string UNITS = "";
        [STDFOptional] public string UNITS_IN = "";
        [STDFOptional] public string C_RESFMT = "";
        [STDFOptional] public string C_LLMFMT = "";
        [STDFOptional] public string C_HLMFMT = "";
        [STDFOptional] public float? LO_SPEC;
        [STDFOptional] public float? HI_SPEC;

        // non-serialized fields used for serialization logic
        public sbyte? ORIG_RES_SCAL;
        public sbyte? ORIG_LLM_SCAL;
        public sbyte? ORIG_HLM_SCAL;
        public float? ORIG_LO_LIMIT;
        public float? ORIG_HI_LIMIT;
        public float? ORIG_START_IN;
        public float? ORIG_INCR_IN;
        public ushort[] ORIG_RTN_INDX;
        public string ORIG_UNITS;
        public string ORIG_UNITS_IN;
        public string ORIG_C_RESFMT;
        public string ORIG_C_LLMFMT;
        public string ORIG_C_HLMFMT;
        public float? ORIG_LO_SPEC;
        public float? ORIG_HI_SPEC;

        public override string ToString()
        {
            return string.Format("** Test# {0},{1},{2},{3}", TEST_NUM, HEAD_NUM, SITE_NUM, "Multiresult Parametric");
        }
    }
}
