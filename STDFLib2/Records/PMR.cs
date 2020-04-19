namespace STDFLib2
{
    /// <summary>
    /// Pin Map Record
    /// </summary>
    public class PMR : STDFRecord
    {
        public PMR() : base((ushort)RecordTypes.PMR) { }
        [STDF] public ushort PMR_INDX = 0;
        [STDF] public ushort CHAN_TYP = 0;
        [STDF] public string CHAN_NAM = "";
        [STDF] public string PHY_NAM = "";
        [STDF] public string LOG_NAM = "";
        [STDF] public byte HEAD_NUM = 1;
        [STDF] public byte SITE_NUM = 1;
    }

}
