﻿using System;

namespace STDFLib2
{
    /// <summary>
    /// Wafer Information Record
    /// </summary>
    public class WIR : STDFRecord
    {
        public WIR() : base(RecordTypes.WIR, "Wafer Information Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0x01;
        [STDF] public byte SITE_GRP { get; set; } = 0xFF;
        [STDF] public DateTime START_T { get; set; } = DateTime.UnixEpoch;
        [STDFOptional] public string WAFER_ID { get; set; } = "";
    }
}