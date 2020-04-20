namespace STDFLib
{
    /// <summary>
    /// Soft Bin Record
    /// </summary>
    public class SBR : STDFRecord, ITestBin
    {
        public SBR() : base(RecordTypes.SBR) { }
        [STDF] public byte HEAD_NUM = 1;
        [STDF] public byte SITE_NUM = 1;
        [STDF] public ushort SBIN_NUM;
        [STDF] public uint SBIN_CNT = 0;
        [STDF] public char SBIN_PF = ' ';
        [STDF] public string SBIN_NAM = "";

        // explicit interface methods
        byte ITestBin.HEAD_NUM { get => HEAD_NUM; set => HEAD_NUM = value; }
        byte ITestBin.SITE_NUM { get => SITE_NUM; set => SITE_NUM = value; }
        ushort ITestBin.BIN_NUM { get => SBIN_NUM; set => SBIN_NUM = value; }
        uint ITestBin.BIN_CNT { get => SBIN_CNT; set => SBIN_CNT = value; }
        char ITestBin.BIN_PF { get => SBIN_PF; set => SBIN_PF = value; }
        string ITestBin.BIN_NAM { get => SBIN_NAM; set => SBIN_NAM = value; }
    }
}
