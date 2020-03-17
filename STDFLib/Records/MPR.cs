namespace STDFLib
{
    /// <summary>
    /// Multi-result Parametric Record
    /// </summary>
    public class MPR : STDFRecord
    {
        protected override RecordType TypeCode => 0x1515;

        [STDF(Order = 1)]
        public uint TEST_NUM { get; set; } = 0xFFFFFFFF;

        [STDF(Order = 2)]
        public byte HEAD_NUM { get; set; } = 0x01;

        [STDF(Order = 3)]
        public byte SITE_NUM { get; set; } = 0x01;

        [STDF(Order = 4)]
        public byte TEST_FLG { get; set; } = (byte)TestResultFlags.InvalidResult;

        [STDF(Order = 5)]
        public byte PARM_FLG { get; set; } = 0;

        [STDF(Order = 6)]
        public ushort RTN_ICNT
        {
            get => (ushort)RTN_STAT.Count;

            set
            {
                // Do nothing, only here for serialization
            }
        }

        [STDF(Order = 7)]
        public ushort RSLT_CNT
        {
            get => (ushort)RTN_RSLT.Length;

            set
            {
                // Do nothing, only here for serialization
            }
        }

        [STDF(Order = 8)]
        public Nibbles RTN_STAT { get; set; } = new Nibbles();

        [STDF(Order = 9)]
        public float[] RTN_RSLT { get; set; } = new float[] { };

        [STDF(Order = 10)]
        public string TEST_TXT { get; set; } = "";

        [STDF(Order = 11)]
        public string ALARM_ID { get; set; } = "";

        [STDF(Order = 12)]
        public byte OPT_FLAG { get; set; } = 0;

        [STDF(Order = 13)]
        public sbyte RES_SCAL { get; set; } = 0;

        [STDF(Order = 14)]
        public sbyte LLM_SCAL { get; set; } = 0;

        [STDF(Order = 15)]
        public sbyte HLM_SCAL { get; set; } = 0;

        [STDF(Order = 16)]
        public float LO_LIMIT { get; set; } = 0;

        [STDF(Order = 17)]
        public float HI_LIMIT { get; set; } = 0;

        [STDF(Order = 18)]
        public float START_IN { get; set; } = 0;

        [STDF(Order = 19)]
        public float INCR_IN { get; set; } = 0;

        [STDF(Order = 20)]
        public ushort[] RTN_INDX { get; set; } = new ushort[] { };

        [STDF(Order = 21)]
        public string UNITS { get; set; } = "";

        [STDF(Order = 22)]
        public string UNITS_IN { get; set; } = "";

        [STDF(Order = 23)]
        public string C_RESFMT { get; set; } = "";

        [STDF(Order = 24)]
        public string C_LLMFMT { get; set; } = "";

        [STDF(Order = 25)]
        public string C_HLMFMT { get; set; } = "";

        [STDF(Order = 26)]
        public float LO_SPEC { get; set; } = 0;

        [STDF(Order = 27)]
        public float HI_SPEC { get; set; } = 0;

        public override string Description => "Multi-result Parametric Record";
    }

}
