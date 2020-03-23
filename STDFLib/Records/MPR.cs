namespace STDFLib
{
    /// <summary>
    /// Multi-result Parametric Record
    /// </summary>
    public class MPR : STDFRecord
    {
        public MPR() : base(RecordTypes.MPR, "Multiresult Parametric Record") { }

        [STDF] public uint TEST_NUM { get; set; } = 0xFFFFFFFF;

        [STDF] public byte HEAD_NUM { get; set; } = 0x01;

        [STDF] public byte SITE_NUM { get; set; } = 0x01;

        [STDF] public byte TEST_FLG { get; set; } = (byte)TestResultFlags.InvalidResult;

        [STDF] public byte PARM_FLG { get; set; } = 0;

        [STDF] public ushort RTN_ICNT
        {
            get => (ushort)RTN_STAT.Count;

            set
            {
                RTN_STAT.Clear();
                RTN_STAT.AddRange(new byte[value]);
            }
        }

        [STDF] public ushort RSLT_CNT
        {
            get => (ushort)RTN_RSLT.Length;

            set
            {
                RTN_RSLT = new float[value];
            }
        }

        [STDF("RTN_ICNT")] public Nibbles RTN_STAT { get; set; } = new Nibbles();

        [STDF("RSLT_CNT")] public float[] RTN_RSLT { get; set; } = new float[] { };

        [STDF] public string TEST_TXT { get; set; } = "";

        [STDF] public string ALARM_ID { get; set; } = "";

        [STDF] public byte OPT_FLAG { get; set; } = 0;

        [STDF] public sbyte RES_SCAL { get; set; } = 0;

        [STDF] public sbyte LLM_SCAL { get; set; } = 0;

        [STDF] public sbyte HLM_SCAL { get; set; } = 0;

        [STDF] public float LO_LIMIT { get; set; } = 0;

        [STDF] public float HI_LIMIT { get; set; } = 0;

        [STDF] public float START_IN { get; set; } = 0;

        [STDF] public float INCR_IN { get; set; } = 0;

        [STDF("RTN_ICNT")] public ushort[] RTN_INDX { get; set; } = new ushort[] { };

        [STDF] public string UNITS { get; set; } = "";

        [STDF] public string UNITS_IN { get; set; } = "";

        [STDF] public string C_RESFMT { get; set; } = "";

        [STDF] public string C_LLMFMT { get; set; } = "";

        [STDF] public string C_HLMFMT { get; set; } = "";

        [STDF] public float LO_SPEC { get; set; } = 0;

        [STDF] public float HI_SPEC { get; set; } = 0;
    }
}
