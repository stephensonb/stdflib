using System;

namespace STDFLib
{
    /// <summary>
    /// Wafer Results Record
    /// </summary>
    public class WRR : STDFRecord
    {
        public override RecordType TypeCode => 0x0214;

        [STDF(Order = 1)]
        public byte HEAD_NUM { get; set; } = 0x01;

        [STDF(Order = 2)]
        public byte SITE_GRP { get; set; } = 0xFF;

        [STDF(Order = 3)]
        public DateTime FINISH_T { get; set; } = DateTime.Now;

        [STDF(Order = 4)]
        public uint PART_CNT { get; set; } = 0;

        [STDF(Order = 5)]
        public uint RTST_CNT { get; set; } = 0xFFFFFFFF;

        [STDF(Order = 6)]
        public uint ABRT_CNT { get; set; } = 0xFFFFFFFF;

        [STDF(Order = 7)]
        public uint GOOD_CNT { get; set; } = 0xFFFFFFFF;

        [STDF(Order = 8)]
        public uint FUNC_CNT { get; set; } = 0xFFFFFFFF;

        [STDF(Order = 9)]
        public string WAFER_ID { get; set; } = "";

        [STDF(Order = 10)]
        public string FABWF_ID { get; set; } = "";

        [STDF(Order = 11)]
        public string FRAME_ID { get; set; } = "";

        [STDF(Order = 12)]
        public string MASK_ID { get; set; } = "";

        [STDF(Order = 13)]
        public string USR_DESC { get; set; } = "";

        [STDF(Order = 14)]
        public string EXC_DESC { get; set; } = "";

        public override string Description => "Wafer Results Record";

    }
}
