namespace STDFLib
{
    /// <summary>
    /// Pin List Record
    /// </summary>
    public class PLR : STDFRecord
    {
        public PLR() : base(RecordTypes.PLR) { }
        [STDF] public ushort GRP_CNT = 0;
        [STDF("GRP_CNT")] public ushort[] GRP_INDX;
        [STDF("GRP_CNT")] public ushort[] GRP_MODE;
        [STDF("GRP_CNT")] public byte[] GRP_RADX;
        [STDF("GRP_CNT")] public string[] RTN_CHAR;
        [STDF("GRP_CNT")] public string[] PGM_CHAR;
        [STDF("GRP_CNT")] public string[] PGM_CHAL;
        [STDF("GRP_CNT")] public string[] RTN_CHAL;
    }
}
