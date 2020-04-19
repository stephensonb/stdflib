namespace STDFLib2
{
    /// <summary>
    /// Pin Group Record
    /// </summary>
    public class PGR : STDFRecord
    {
        public PGR() : base(RecordTypes.PGR, "Pin Group Record") { }

        [STDF] public ushort GRP_INDX { get; set; } = 0;
        [STDF] public string GRP_NAM { get; set; } = "";
        [STDF] public ushort INDX_CNT { get => (ushort)(PMR_INDX?.Length ?? 0); set => PMR_INDX = new ushort[value]; }
        [STDFOptional] public ushort[] PMR_INDX { get; set; } = new ushort[0];
    }
}
