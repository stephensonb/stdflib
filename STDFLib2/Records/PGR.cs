namespace STDFLib2
{
    /// <summary>
    /// Pin Group Record
    /// </summary>
    public class PGR : STDFRecord
    {
        public PGR() : base((ushort)RecordTypes.PGR) { }

        [STDF] public ushort GRP_INDX;
        [STDF] public string GRP_NAM = "";
        [STDF] public ushort INDX_CNT = 0;
        [STDF("INDX_CNT")] public ushort[] PMR_INDX;
    }
}
