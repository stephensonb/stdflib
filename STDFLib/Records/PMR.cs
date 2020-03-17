using System;

namespace STDFLib
{
    /// <summary>
    /// Pin Map Record
    /// </summary>
    public class PMR : STDFRecord
    {
        private ushort _pmr_indx = 0;

        protected override RecordType TypeCode => 0x0160;

        [STDF(Order = 1)]
        public ushort PMR_INDX
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

        [STDF(Order = 2)]
        public ushort CHAN_TYP { get; set; } = 0;

        [STDF(Order = 3)]
        public string CHAN_NAM { get; set; } = "";

        [STDF(Order = 4)]
        public string PHY_NAM { get; set; } = "";

        [STDF(Order = 5)]
        public string LOG_NAM { get; set; } = "";

        [STDF(Order = 6)]
        public byte HEAD_NUM { get; set; } = 0x01;

        [STDF(Order = 7)]
        public byte SITE_NUM { get; set; } = 0x01;

        public override string Description => "Pin Map Record";
    }
}
