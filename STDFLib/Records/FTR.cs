namespace STDFLib
{
    /// <summary>
    /// Functional Test Record
    /// </summary>
    public class FTR : STDFRecord
    {
        // supports defaults
        public FTR() : base(RecordTypes.FTR) { }

        [STDF] public uint TEST_NUM = 0;
        [STDF] public byte HEAD_NUM = 1;
        [STDF] public byte SITE_NUM = 1;
        [STDF] public byte TEST_FLG = 0;
        [STDF] public byte OPT_FLAG = 0;
        [STDF] public uint? CYCL_CNT;
        [STDF] public uint? REL_VADR;
        [STDF] public uint? REPT_CNT;
        [STDF] public uint? NUM_FAIL;
        [STDF] public int? XFAIL_AD;
        [STDF] public int? YFAIL_AD;
        [STDF] public short? VECT_OFF;
        [STDF] public ushort? RTN_ICNT;
        [STDF] public ushort? PGM_ICNT;
        [STDF("RTN_ICNT")] public ushort[] RTN_INDX;
        [STDF("RTN_ICNT")] public byte[] RTN_STAT;
        [STDF("PGM_ICNT")] public ushort[] PGM_INDX;
        [STDF("PGM_ICNT")] public byte[] PGM_STAT;
        [STDF] public BitArray FAIL_PIN;
        [STDF] public string VECT_NAM = "";
        [STDF] public string TIME_SET = "";
        [STDF] public string OP_CODE = "";
        [STDF] public string TEST_TXT = "";
        [STDF] public string ALARM_ID = "";
        [STDF] public string PROG_TXT = "";
        [STDF] public string RSLT_TXT = "";
        [STDFOptional] public byte? PATG_NUM = 255;
        [STDFOptional] public ByteArray SPIN_MAP;

        // non-serialized members
        public byte? ORIG_PATG_NUM;
        public ByteArray ORIG_SPIN_MAP;

        public override string ToString()
        {
            return string.Format("** Test# {0},{1},{2},{3}", TEST_NUM, HEAD_NUM, SITE_NUM, "Functional");
        }
    }
}