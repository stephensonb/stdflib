namespace STDFLib2
{
    /// <summary>
    /// Retest Data Record
    /// </summary>
    public class RDR : STDFRecord
    {
        public RDR() : base((ushort)RecordTypes.RDR) { }

        [STDF] public ushort NUM_BINS = 0;
        [STDF("NUM_BINS")] public ushort[] RTST_BIN;
    }
}
