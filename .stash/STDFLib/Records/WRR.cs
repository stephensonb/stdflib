using System;

namespace STDFLib2
{
    /// <summary>
    /// Wafer Results Record
    /// </summary>
    public class WRR : STDFRecord
    {
        public WRR() : base(RecordTypes.WRR, "Wafer Results Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0x00;
        [STDF] public byte SITE_GRP { get; set; } = 0xFF;
        [STDF] public DateTime FINISH_T { get; set; } = DateTime.UnixEpoch;
        [STDF] public uint PART_CNT { get; set; } = 0;
        [STDFOptional] public uint RTST_CNT { get; set; } = 0xFFFFFFFF;
        [STDFOptional] public uint ABRT_CNT { get; set; } = 0xFFFFFFFF;
        [STDFOptional] public uint GOOD_CNT { get; set; } = 0xFFFFFFFF;
        [STDFOptional] public uint FUNC_CNT { get; set; } = 0xFFFFFFFF;
        [STDFOptional] public string WAFER_ID { get; set; } = "";
        [STDFOptional] public string FABWF_ID { get; set; } = "";
        [STDFOptional] public string FRAME_ID { get; set; } = "";
        [STDFOptional] public string MASK_ID { get; set; } = "";
        [STDFOptional] public string USR_DESC { get; set; } = "";
        [STDFOptional] public string EXC_DESC { get; set; } = "";
    }
}
