namespace STDFLib2
{
    /// <summary>
    /// Parametric Test Record
    /// </summary>
    public class PTR : STDFRecord
    {
        public PTR() : base((ushort)RecordTypes.PTR,true) { }

        [STDF] public uint TEST_NUM = 0;
        [STDF] public byte HEAD_NUM = 1;
        [STDF] public byte SITE_NUM = 1;
        [STDF] public byte TEST_FLG;
        [STDF] public byte PARM_FLG;
        [STDF] public float? RESULT;
        [STDF] public string TEST_TXT = "";
        [STDF] public string ALARM_ID = "";
        [STDFOptional] public byte OPT_FLAG;
        [STDFOptional] public sbyte? RES_SCAL;
        [STDFOptional] public sbyte? LLM_SCAL;
        [STDFOptional] public sbyte? HLM_SCAL;
        [STDFOptional] public float? LO_LIMIT;
        [STDFOptional] public float? HI_LIMIT;
        [STDFOptional] public string UNITS    = "";
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
        public string ORIG_UNITS;
        public string ORIG_C_RESFMT;
        public string ORIG_C_LLMFMT;
        public string ORIG_C_HLMFMT;
        public float? ORIG_LO_SPEC;
        public float? ORIG_HI_SPEC;

        public override string ToString()
        {
            return string.Format("** Test# {0},{1},{2},{3}", TEST_NUM, HEAD_NUM, SITE_NUM, "Parametric");
        }
    }
}
