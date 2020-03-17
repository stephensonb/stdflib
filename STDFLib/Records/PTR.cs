namespace STDFLib
{
    /// <summary>
    /// Parametric Test Record
    /// </summary>
    public class PTR : STDFRecord
    {
        protected override RecordType TypeCode => 0x1510;

        [STDF(Order = 1)]
        public uint TEST_NUM { get; set; } = 0xFFFFFFFF;

        [STDF(Order = 2)]
        public byte HEAD_NUM { get; set; } = 0x01;

        [STDF(Order = 3)]
        public byte SITE_NUM { get; set; } = 0x01;

        [STDF(Order = 4)]
        public byte TEST_FLG { get; set; } = 0;

        [STDF(Order = 5)]
        public byte PARM_FLG { get; set; } = 0x00;

        [STDF(Order = 6)]
        public float RESULT { get; set; } = 0;

        [STDF(Order = 7)]
        public string TEST_TXT { get; set; } = "";

        [STDF(Order = 8)]
        public string ALARM_ID { get; set; } = "";

        [STDF(Order = 9)]
        public byte OPT_FLAG { get; set; } = (byte)PTROptionalData.AllOptionalDataInvalid;

        [STDF(Order = 10)]
        public sbyte RES_SCAL { get; set; }

        [STDF(Order = 11)]
        public sbyte LLM_SCAL { get; set; }

        [STDF(Order = 12)]
        public sbyte HLM_SCAL { get; set; }

        [STDF(Order = 13)]
        public float LO_LIMIT { get; set; }

        [STDF(Order = 14)]
        public float HI_LIMIT { get; set; }

        [STDF(Order = 15)]
        public string UNITS { get; set; }

        [STDF(Order = 16)]
        public string C_RESFMT { get; set; }

        [STDF(Order = 17)]
        public string C_LLMFMT { get; set; }

        [STDF(Order = 18)]
        public string C_HLMFMT { get; set; }

        [STDF(Order = 19)]
        public float LO_SPEC { get; set; }

        [STDF(Order = 20)]
        public float HI_SPEC { get; set; }

        public override string Description => "Parametric Test Record";
    }
}
