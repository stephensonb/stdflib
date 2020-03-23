using System;

namespace STDFLib
{
    /// <summary>
    /// Pin Map Record
    /// </summary>
    public class PMR : STDFRecord
    {
        private ushort _pmr_indx = 0;

        public PMR() : base(RecordTypes.PMR, "Pin Map Record") { }

        [STDF] public ushort PMR_INDX
        {
            get => _pmr_indx;

            set
            {
                if (value < 1 || value > 32767)
                {
                    throw new ArgumentException(string.Format("Invalid Pin Map Index.  Value must be between 1 and 32,767.  Value received was {0}.", value));
                }
            }
        }

        [STDF] public ushort CHAN_TYP { get; set; } = 0;

        [STDF] public string CHAN_NAM { get; set; } = "";

        [STDF] public string PHY_NAM { get; set; } = "";

        [STDF] public string LOG_NAM { get; set; } = "";

        [STDF] public byte HEAD_NUM { get; set; } = 0x01;

        [STDF] public byte SITE_NUM { get; set; } = 0x01;
    }
}
