namespace STDFLib2
{
    /// <summary>
    /// Wafer Configuration Record
    /// </summary>
    public class WCR : STDFRecord
    {
        public WCR() : base(RecordTypes.WCR, "Wafer Configuration Record") { }

        [STDFOptional] public float WAFR_SIZ { get; set; } = 0;
        [STDFOptional] public float DIE_HT { get; set; } = 0;
        [STDFOptional] public float DIE_WID { get; set; } = 0;
        [STDFOptional] public byte WF_UNITS { get; set; } = 0;
        [STDFOptional] public char WF_FLAT { get; set; } = ' ';
        [STDFOptional] public short CENTER_X { get; set; } = -32768;
        [STDFOptional] public short CENTER_Y { get; set; } = -32768;
        [STDFOptional] public char POS_X { get; set; } = ' ';
        [STDFOptional] public char POS_Y { get; set; } = ' ';
    }
}
