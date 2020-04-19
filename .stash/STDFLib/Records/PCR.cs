namespace STDFLib2
{
    /// <summary>
    /// Part Count Record
    /// </summary>
    public class PCR : STDFRecord
    {
        public PCR() : base(RecordTypes.PCR, "Part Count Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = byte.MaxValue;
        [STDF] public byte SITE_NUM { get; set; } = 0x00;
        [STDF] public uint PART_CNT { get; set; } = 0x00;
        [STDFOptional] public uint? RTST_CNT { get; set; } = uint.MaxValue;
        [STDFOptional] public uint? ABRT_CNT { get; set; } = uint.MaxValue;
        [STDFOptional] public uint? GOOD_CNT { get; set; } = uint.MaxValue;
        [STDFOptional] public uint? FUNC_CNT { get; set; } = uint.MaxValue;
    }
}
