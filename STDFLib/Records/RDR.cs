namespace STDFLib
{
    /// <summary>
    /// Retest Data Record
    /// </summary>
    public class RDR : STDFRecord
    {
        public RDR() : base(RecordTypes.RDR) { }

        [STDF] public ushort NUM_BINS = 0;
        [STDF("NUM_BINS")] public ushort[] RTST_BIN;
    }
}
