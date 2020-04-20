namespace STDFLib
{
    /// <summary>
    /// Wafer Configuration Record
    /// </summary>
    public class WCR : STDFRecord
    {
        public WCR() : base(RecordTypes.WCR) { }
        [STDF] public float? WAFR_SIZ;
        [STDF] public float? DIE_HT;
        [STDF] public float? DIE_WID;
        [STDF] public byte? WF_UNITS;
        [STDF] public char WF_FLAT = ' ';
        [STDF] public short CENTER_X = -32768;
        [STDF] public short CENTER_Y = -32768;
        [STDF] public char POS_X = ' ';
        [STDF] public char POS_Y = ' ';
    }
}
