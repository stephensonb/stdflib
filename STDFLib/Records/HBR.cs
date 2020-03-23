using System;

namespace STDFLib
{
    /// <summary>
    /// Hard Bin Record
    /// </summary>
    public class HBR : STDFRecord
    {
        private char _hbin_pf = ' ';

        public HBR() : base(RecordTypes.HBR, "Hard Bin Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0xFF;

        [STDF] public byte SITE_NUM { get; set; } = 0x01;

        [STDF] public ushort HBIN_NUM { get; set; } = 0;

        [STDF] public uint HBIN_CNT { get; set; } = 0;

        [STDF] public char HBIN_PF
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
                    case '\0':
                        _hbin_pf = ' ';
                        break;
                    default:
                        throw new ArgumentException(string.Format("Unsupported Hard Bin Pass/Fail value (HBIN_PF) value passed.  Valid values are Y, N or a space.  Value received was '{0}'.", value));
                }
            }
        }

        [STDF] public string HBIN_NAM { get; set; }
    }

}
