using System;

namespace STDFLib
{
    /// <summary>
    /// Master Results Record
    /// </summary>
    public class MRR : STDFRecord
    {
        private char _disp_cod = ' ';
        public override RecordType TypeCode => 0x0114;

        [STDF(Order = 1)]
        public DateTime FINISH_T { get; set; } = DateTime.Now;

        [STDF(Order = 2)]
        public char DISP_COD
        {
            get => _disp_cod;

            set
            {
                if (!char.IsLetterOrDigit(value) && value != ' ')
                {
                    throw new ArgumentException(string.Format("Unsupported Disposition Code (DISP_COD) value.  Valid values are 0-9, A-Z or a space.  Value received was '{0}'.", value));
                }
                _disp_cod = value;
            }
        }

        [STDF(Order = 3)]
        public string USR_DESC { get; set; } = "";

        [STDF(Order = 4)]
        public string EXC_DESC { get; set; } = "";

        public override string Description => "Master Results Record";
    }
}
