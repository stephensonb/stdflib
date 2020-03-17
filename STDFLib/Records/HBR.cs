using System;

namespace STDFLib
{
    /// <summary>
    /// Hard Bin Record
    /// </summary>
    public class HBR : STDFRecord
    {
        private char _hbin_pf = ' ';

        protected override RecordType TypeCode => 0x0140;

        [STDF(Order = 1)]
        public byte HEAD_NUM { get; set; } = 0xFF;

        [STDF(Order = 2)]
        public byte SITE_NUM { get; set; } = 0x01;

        [STDF(Order = 3)]
        public ushort HBIN_NUM { get; set; } = 0;

        [STDF(Order = 4)]
        public uint HBIN_CNT { get; set; } = 0;

        [STDF(Order = 5)]
        public char HBIN_PF
        {
            get => _hbin_pf;

            set
            {
                switch(char.ToUpper(value))
                {
                    case 'Y':
                    case 'N':
                    case ' ':
                        _hbin_pf = value;
                        break;
                    default:
                        throw new ArgumentException(string.Format("Unsupported Hard Bin Pass/Fail value (HBIN_PF) value passed.  Valid values are Y, N or a space.  Value received was '{0}'.", value));
                }
            }
        }

        [STDF(Order = 6)]
        public string HBIN_NAM { get; set; }

        public override string Description => "Hard Bin Record";
    }

}
