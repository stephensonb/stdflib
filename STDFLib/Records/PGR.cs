namespace STDFLib
{
    /// <summary>
    /// Pin Group Record
    /// </summary>
    public class PGR : STDFRecord
    {
        public PGR() : base(RecordTypes.PGR) { }

        [STDF] public ushort GRP_INDX;
        [STDF] public string GRP_NAM = "";
        [STDF] public ushort INDX_CNT = 0;
        [STDF("INDX_CNT")] public ushort[] PMR_INDX;
    }
}
