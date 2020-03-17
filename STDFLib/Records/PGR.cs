using System;
using System.Linq;

namespace STDFLib
{
    /// <summary>
    /// Pin Group Record
    /// </summary>
    public class PGR : STDFRecord
    {
        private ushort _grp_indx = 32768;

        protected override RecordType TypeCode => 0x0162;

        [STDF(Order = 1)]
        public ushort GRP_INDX
        {
            get => _grp_indx;

            set
            {
                if (value < 32768)
                {
                    throw new ArgumentException(string.Format("Invalid Pin Group Index.  Value must be between 32,768 and 65,535.  Value received was {0}.", value));
                }
            }
        }

        [STDF(Order = 1)]
        public string GRP_NAM { get; set; } = "";

        [STDF(Order = 1)]
        public ushort INDX_CNT
        {
            get => (ushort)PMR_INDX.Length;

            set
            {
                // do nothing.  No need to set the count here.  this is only here for deserialization purposes
            }
        }

        [STDF(Order = 1, ItemCountProvider = "INDX_CNT")]
        public ushort[] PMR_INDX { get; set; } = new ushort[] { };

        public override string Description => "Pin Group Record";

    }
}
