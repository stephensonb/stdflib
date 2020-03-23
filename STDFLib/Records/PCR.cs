namespace STDFLib
{
    /// <summary>
    /// Part Count Record
    /// </summary>
    public class PCR : STDFRecord
    {
        public PCR() : base(RecordTypes.PCR, "Part Count Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0xFF;

        [STDF] public byte SITE_NUM { get; set; } = 0x01;

        [STDF] public uint PART_CNT { get; set; } = 0x00;

        [STDF] public uint RTST_CNT { get; set; } = 0xFFFFFFFF;

        [STDF] public uint ABRT_CNT { get; set; } = 0xFFFFFFFF;

        [STDF] public uint GOOD_CNT { get; set; } = 0xFFFFFFFF;

        [STDF] public uint FUNC_CNT { get; set; } = 0xFFFFFFFF;
    }
}
