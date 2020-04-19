using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace STDFLib2
{
    /// <summary>
    /// Hard Bin Record
    /// </summary>
    public class HBR : STDFRecord, ITestBin
    {
        public HBR() : base((ushort)RecordTypes.HBR) { }

        [STDF] public byte HEAD_NUM = 1;
        [STDF] public byte SITE_NUM = 1;
        [STDF] public ushort HBIN_NUM = 0;
        [STDF] public uint HBIN_CNT = 0;
        [STDF] public char HBIN_PF = ' ';
        [STDF] public string HBIN_NAM = "";

        byte ITestBin.HEAD_NUM { get => HEAD_NUM; set => HEAD_NUM = value; }
        byte ITestBin.SITE_NUM { get => SITE_NUM; set => SITE_NUM = value; }
        ushort ITestBin.BIN_NUM { get => HBIN_NUM; set => HBIN_NUM = value; }
        uint ITestBin.BIN_CNT { get => HBIN_CNT; set => HBIN_CNT = value; }
        char ITestBin.BIN_PF { get => HBIN_PF; set => HBIN_PF = value; }
        string ITestBin.BIN_NAM { get => HBIN_NAM; set => HBIN_NAM = value; }
    }
}
