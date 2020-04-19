namespace STDFLib2
{
    /// <summary>
    /// Test Synopsis Record
    /// </summary>
    public class TSR : STDFRecord
    {
        public TSR() : base(RecordTypes.TSR, "Test Synopsis Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0xFF;
        [STDF] public byte SITE_NUM { get; set; } = 0x00;
        [STDF] public char TEST_TYP { get; set; } = ' ';
        [STDF] public uint TEST_NUM { get; set; } = 0;
        [STDFOptional] public uint EXEC_CNT { get; set; } = 0xFFFFFFFF;
        [STDFOptional] public uint FAIL_CNT { get; set; } = 0xFFFFFFFF;
        [STDFOptional] public uint ALRM_CNT { get; set; } = 0xFFFFFFFF;
        [STDFOptional] public string TEST_NAM { get; set; } = "";
        [STDFOptional] public string SEQ_NAM { get; set; } = "";
        [STDFOptional] public string TEST_LBL { get; set; } = "";
        [STDFOptional] public byte OPT_FLAG { get; set; } = (byte)TSROptionalData.AllOptionalDataIsInvalid;
        [STDFOptional] public float? TEST_TIM { get; set; } = null;
        [STDFOptional] public float? TEST_MIN { get; set; } = null;
        [STDFOptional] public float? TEST_MAX { get; set; } = null;
        [STDFOptional] public float? TST_SUMS { get; set; } = null;
        [STDFOptional] public float? TST_SQRS { get; set; } = null;
    }
}
