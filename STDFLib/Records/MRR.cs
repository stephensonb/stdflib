using System;

namespace STDFLib
{
    /// <summary>
    /// Master Results Record
    /// </summary>
    public class MRR : STDFRecord
    {
        public MRR() : base(RecordTypes.MRR) { }

        [STDF] public DateTime FINISH_T;
        [STDF] public char DISP_COD = ' ';
        [STDF] public string USR_DESC = "";
        [STDF] public string EXC_DESC = "";
    }
}
