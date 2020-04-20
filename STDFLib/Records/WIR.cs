using System;

namespace STDFLib
{
    /// <summary>
    /// Wafer Information Record
    /// </summary>
    public class WIR : STDFRecord
    {
        public WIR() : base(RecordTypes.WIR) { }

        [STDF] public byte HEAD_NUM = 1;
        [STDF] public byte SITE_GRP = 255;
        [STDF] public DateTime START_T;
        [STDF] public string WAFER_ID = "";
    }
}
