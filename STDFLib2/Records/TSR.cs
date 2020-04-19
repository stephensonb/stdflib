namespace STDFLib2
{
    /// <summary>
    /// Test Synopsis Record
    /// </summary>
    public class TSR : STDFRecord
    {
        public TSR() : base((ushort)RecordTypes.TSR) { }

        [STDF] public         byte   HEAD_NUM = 1;
        [STDF] public         byte   SITE_NUM = 1;
        [STDF] public         char   TEST_TYP = ' ';
        [STDF] public         uint   TEST_NUM = 0;
        [STDF] public uint   EXEC_CNT = 0xFFFFFFFF;
        [STDF] public uint   FAIL_CNT = 0xFFFFFFFF;
        [STDF] public uint   ALRM_CNT = 0xFFFFFFFF;
        [STDF] public string TEST_NAM = "";
        [STDF] public string SEQ_NAM  = "";
        [STDF] public string TEST_LBL = "";
        [STDF] public byte OPT_FLAG;
        [STDF] public float? TEST_TIM;
        [STDF] public float? TEST_MIN;
        [STDF] public float? TEST_MAX;
        [STDF] public float? TST_SUMS;
        [STDF] public float? TST_SQRS;
    }
}
