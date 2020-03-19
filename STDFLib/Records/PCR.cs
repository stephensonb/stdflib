namespace STDFLib
{
    /// <summary>
    /// Part Count Record
    /// </summary>
    public class PCR : STDFRecord
    {
        public override RecordType TypeCode => 0x011E;

        [STDF(Order = 1)]
        public byte HEAD_NUM { get; set; } = 0xFF;

        [STDF(Order = 2)]
        public byte SITE_NUM { get; set; } = 0x01;

        [STDF(Order = 3)]
        public uint PART_CNT { get; set; } = 0x00;

        [STDF(Order = 4)]
        public uint RTST_CNT { get; set; } = 0xFFFFFFFF;

        [STDF(Order = 5)]
        public uint ABRT_CNT { get; set; } = 0xFFFFFFFF;

        [STDF(Order = 6)]
        public uint GOOD_CNT { get; set; } = 0xFFFFFFFF;

        [STDF(Order = 7)]
        public uint FUNC_CNT { get; set; } = 0xFFFFFFFF;

        public override string Description => "Part Count Record";

    }
}
