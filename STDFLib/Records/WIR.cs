using System;

namespace STDFLib
{
    /// <summary>
    /// Wafer Information Record
    /// </summary>
    public class WIR : STDFRecord
    {
        protected override RecordType TypeCode => 0x0210;

        [STDF(Order = 1)]
        public byte HEAD_NUM { get; set; } = 0x01;

        [STDF(Order = 2)]
        public byte SITE_GRP { get; set; } = 0xFF;

        [STDF(Order = 3)]
        public DateTime START_T { get; set; } = DateTime.Now;

        [STDF(Order = 4)]
        public string WAFER_ID { get; set; } = "";

        public override string Description => "Wafer Information Record";
    }
}
