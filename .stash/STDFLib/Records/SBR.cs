namespace STDFLib2.Serialization
{
    /// <summary>
    /// Soft Bin Record
    /// </summary>
    public class SBR : STDFRecord, ITestBin
    {
        public SBR() : base(RecordTypes.SBR, "Soft Bin Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0xFF;
        [STDF] public byte SITE_NUM { get; set; } = 0x00;
        [STDF] public ushort SBIN_NUM { get; set; } = 0;
        [STDF] public uint SBIN_CNT { get; set; } = 0;
        [STDFOptional] public char SBIN_PF { get; set; } = ' ';
        [STDFOptional] public string SBIN_NAM { get; set; } = "";

        // explicit interface methods
        byte ITestBin.HEAD_NUM { get => HEAD_NUM; set => HEAD_NUM = value; }
        byte ITestBin.SITE_NUM { get => SITE_NUM; set => SITE_NUM = value; }
        ushort ITestBin.BIN_NUM { get => SBIN_NUM; set => SBIN_NUM = value; }
        uint ITestBin.BIN_CNT { get => SBIN_CNT; set => SBIN_CNT = value; }
        char ITestBin.BIN_PF { get => SBIN_PF; set => SBIN_PF = value; }
        string ITestBin.BIN_NAM { get => SBIN_NAM; set => SBIN_NAM = value; }
    }
}
