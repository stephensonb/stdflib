namespace STDFLib
{
    /// <summary>
    /// Retest Data Record
    /// </summary>
    public class RDR : STDFRecord
    {
        protected override RecordType TypeCode => 0x0170;

        [STDF(Order = 1)]
        public ushort NUM_BINS
        {
            get => (ushort)RTST_BIN.Length;

            set
            {
                // do nothing.  only here for serialization
            }
        }

        [STDF(Order = 2)]
        public ushort[] RTST_BIN { get; set; } = new ushort[] { };

        public override string Description => "Retest Data Record";

    }
}
