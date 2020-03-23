namespace STDFLib
{
    /// <summary>
    /// Retest Data Record
    /// </summary>
    public class RDR : STDFRecord
    {
        public RDR() : base(RecordTypes.RDR, "Retest Data Record") { }

        [STDF] public ushort NUM_BINS
        {
            get => (ushort)RTST_BIN.Length;

            set
            {
                RTST_BIN = new ushort[value];
            }
        }

        [STDF("NUM_BINS")] public ushort[] RTST_BIN { get; set; } = new ushort[] { };
    }
}
