using System;

namespace STDFLib2
{
    /// <summary>
    /// Wafer Results Record
    /// </summary>
    public class WRR : STDFRecord
    {
        public WRR() : base((ushort)RecordTypes.WRR) { }

        [STDF] public byte HEAD_NUM = 1;
        [STDF] public byte SITE_GRP = 255;
        [STDF] public DateTime FINISH_T;
        [STDF] public uint PART_CNT = 0;
        [STDF] public uint RTST_CNT = 0xFFFFFFFF;
        [STDF] public uint ABRT_CNT = 0xFFFFFFFF;
        [STDF] public uint GOOD_CNT = 0xFFFFFFFF;
        [STDF] public uint FUNC_CNT = 0xFFFFFFFF;
        [STDF] public string WAFER_ID = "";
        [STDF] public string FABWF_ID = "";
        [STDF] public string FRAME_ID = "";
        [STDF] public string MASK_ID  = "";
        [STDF] public string USR_DESC = "";
        [STDF] public string EXC_DESC = "";
    }
}
