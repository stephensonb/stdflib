using System;

namespace STDFLib
{
    /// <summary>
    /// Wafer Results Record
    /// </summary>
    public class WRR : STDFRecord
    {
        public WRR() : base(RecordTypes.WRR, "Wafer Results Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0x01;

        [STDF] public byte SITE_GRP { get; set; } = 0xFF;

        [STDF] public DateTime FINISH_T { get; set; } = DateTime.Now;

        [STDF] public uint PART_CNT { get; set; } = 0;

        [STDF] public uint RTST_CNT { get; set; } = 0xFFFFFFFF;

        [STDF] public uint ABRT_CNT { get; set; } = 0xFFFFFFFF;

        [STDF] public uint GOOD_CNT { get; set; } = 0xFFFFFFFF;

        [STDF] public uint FUNC_CNT { get; set; } = 0xFFFFFFFF;

        [STDF] public string WAFER_ID { get; set; } = "";

        [STDF] public string FABWF_ID { get; set; } = "";

        [STDF] public string FRAME_ID { get; set; } = "";

        [STDF] public string MASK_ID { get; set; } = "";

        [STDF] public string USR_DESC { get; set; } = "";

        [STDF] public string EXC_DESC { get; set; } = "";
    }
}
