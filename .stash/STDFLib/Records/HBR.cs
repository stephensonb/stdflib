using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace STDFLib2.Serialization
{
    /// <summary>
    /// Hard Bin Record
    /// </summary>
    public class HBR : STDFRecord, ITestBin
    {
        public HBR() : base(RecordTypes.HBR, "Hard Bin Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0xFF;
        [STDF] public byte SITE_NUM { get; set; } = 0x00;
        [STDF] public ushort HBIN_NUM { get; set; } = 0;
        [STDF] public uint HBIN_CNT { get; set; } = 0;
        [STDFOptional] public char HBIN_PF { get; set; } = ' ';
        [STDFOptional] public string HBIN_NAM { get; set; } = "";

        byte ITestBin.HEAD_NUM { get => HEAD_NUM; set => HEAD_NUM = value; }
        byte ITestBin.SITE_NUM { get => SITE_NUM; set => SITE_NUM = value; }
        ushort ITestBin.BIN_NUM { get => HBIN_NUM; set => HBIN_NUM = value; }
        uint ITestBin.BIN_CNT { get => HBIN_CNT; set => HBIN_CNT = value; }
        char ITestBin.BIN_PF { get => HBIN_PF; set => HBIN_PF = value; }
        string ITestBin.BIN_NAM { get => HBIN_NAM; set => HBIN_NAM = value; }
    }

}
