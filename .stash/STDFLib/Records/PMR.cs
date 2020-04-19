namespace STDFLib2
{
    /// <summary>
    /// Pin Map Record
    /// </summary>
    public class PMR : STDFRecord
    {
        public PMR() : base(RecordTypes.PMR, "Pin Map Record") { }

        [STDF] public ushort PMR_INDX { get; set; } = 0;
        [STDFOptional] public ushort CHAN_TYP { get; set; } = 0;
        [STDFOptional] public string CHAN_NAM { get; set; } = "";
        [STDFOptional] public string PHY_NAM { get; set; } = "";
        [STDFOptional] public string LOG_NAM { get; set; } = "";
        [STDFOptional] public byte HEAD_NUM { get; set; } = 0x01;
        [STDFOptional] public byte SITE_NUM { get; set; } = 0x01;
    }
}
