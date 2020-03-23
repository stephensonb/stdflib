using System;

namespace STDFLib
{
    /// <summary>
    /// Soft Bin Record
    /// </summary>
    public class SBR : STDFRecord
    {
        private char _sbin_pf = ' ';

        public SBR() : base(RecordTypes.SBR, "Soft Bin Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0xFF;

        [STDF] public byte SITE_NUM { get; set; } = 0x01;

        [STDF] public ushort SBIN_NUM { get; set; } = 0;

        [STDF] public uint SBIN_CNT { get; set; } = 0;

        [STDF] public char SBIN_PF
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

        [STDF] public string SBIN_NAM { get; set; }
    }
}
