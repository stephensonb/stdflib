namespace STDFLib
{
    /// <summary>
    /// Functional Test Record
    /// </summary>
    public class FTR : STDFRecord
    {
        protected override RecordType TypeCode => 0x1520;

        [STDF(Order = 1)]
        public uint TEST_NUM { get; set; } = 0xFFFFFFFF;

        [STDF(Order = 2)]
        public byte HEAD_NUM { get; set; } = 0x01;

        [STDF(Order = 3)]
        public byte SITE_NUM { get; set; } = 0x01;

        [STDF(Order = 4)]
        public byte TEST_FLG { get; set; } = (byte)TestResultFlags.InvalidResult;

        [STDF(Order = 5)]
        public byte OPT_FLG { get; set; } = (byte)FTROptionalData.AllOptionalDataIsInvalid;

        [STDF(Order = 6)]
        public uint CYCL_CNT { get; set; } = 0;

        [STDF(Order = 7)]
        public uint REL_VADR { get; set; } = 0;

        [STDF(Order = 8)]
        public uint REPT_CNT { get; set; } = 0;

        [STDF(Order = 9)]
        public uint NUM_FAIL { get; set; } = 0;

        [STDF(Order = 10)]
        public int XFAIL_AD { get; set; } = 0;

        [STDF(Order = 11)]
        public int YFAIL_AD { get; set; } = 0;

        [STDF(Order = 12)]
        public short VECT_OFF { get; set; } = 0;

        [STDF(Order = 13)]
        public ushort RTN_ICNT
        {
            get => (ushort)RTN_INDX.Length;

            set
            {
                // do nothing, only here for serialization
            }
        }

        [STDF(Order = 14)]
        public ushort PGM_ICNT
        {
            get => (ushort)PGM_INDX.Length;

            set
            {
                // do nothing, only here for serialization
            }
        }

        [STDF(Order = 15)]
        public ushort[] RTN_INDX {get; set;} = new ushort[] { };

        [STDF(Order = 16)]
        public Nibbles RTN_STAT { get; set; } = new Nibbles();

        [STDF(Order = 17)]
        public ushort[] PGM_INDX { get; set; } = new ushort[] { };

        [STDF(Order = 18)]
        public Nibbles PGM_STAT { get; set; } = new Nibbles();

        [STDF(Order = 19)]
        public BitField FAIL_PIN { get; set; } = new BitField();

        [STDF(Order = 20)]
        public string VECT_NAM { get; set; } = "";

        [STDF(Order = 21)]
        public string TIME_SET { get; set; } = "";

        [STDF(Order = 22)]
        public string OP_CODE { get; set; } = "";

        [STDF(Order = 23)]
        public string TEST_TXT { get; set; } = "";

        [STDF(Order = 24)]
        public string ALARM_ID { get; set; } = "";

        [STDF(Order = 25)]
        public string PROG_TXT { get; set; } = "";

        [STDF(Order = 26)]
        public string RSLT_TXT { get; set; } = "";

        [STDF(Order = 27)]
        public byte PATG_NUM { get; set; } = 0;

        [STDF(Order = 28)]
        public BitField SPIN_MAP { get; set; } = new BitField();

        public override string Description => "Functional Test Record";
    }
}
