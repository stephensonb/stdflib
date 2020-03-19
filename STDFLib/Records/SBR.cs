using System;

namespace STDFLib
{
    /// <summary>
    /// Soft Bin Record
    /// </summary>
    public class SBR : STDFRecord
    {
        private char _sbin_pf = ' ';

        public override RecordType TypeCode => 0x0132;

        [STDF(Order = 1)]
        public byte HEAD_NUM { get; set; } = 0xFF;

        [STDF(Order = 2)]
        public byte SITE_NUM { get; set; } = 0x01;

        [STDF(Order = 3)]
        public ushort SBIN_NUM { get; set; } = 0;

        [STDF(Order = 4)]
        public uint SBIN_CNT { get; set; } = 0;

        [STDF(Order = 5)]
        public char SBIN_PF
        {
            get => _sbin_pf;

            set
            {
                switch (char.ToUpper(value))
                {
                    case 'P':
                    case 'F':
                    case ' ':
                        _sbin_pf = value;
                        break;
                    case '\0':
                        _sbin_pf = ' ';
                        break;
                    default:
                        throw new ArgumentException(string.Format("Unsupported Soft Bin Pass/Fail value (SBIN_PF) value.  Valid values are Y, N or a space.  Value received was '{0}'.", value));
                }
            }
        }

        [STDF(Order = 6)]
        public string SBIN_NAM { get; set; }

        public override string Description => "Soft Bin Record";

    }
}
