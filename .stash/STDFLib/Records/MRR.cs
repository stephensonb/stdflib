using System;

namespace STDFLib2
{
    /// <summary>
    /// Master Results Record
    /// </summary>
    public class MRR : STDFRecord
    {
        public MRR() : base(RecordTypes.MRR, "Master Results Record") { }

        [STDF] public DateTime FINISH_T { get; set; } = DateTime.Now;
        [STDFOptional] public char DISP_COD { get; set; } = ' ';
        [STDFOptional] public string USR_DESC { get; set; } = "";
        [STDFOptional] public string EXC_DESC { get; set; } = "";
    }
}
