using System;
using System.Collections;
using System.Linq;

namespace STDFLib2
{
    /// <summary>
    /// Pin List Record
    /// </summary>
    public class PLR : STDFRecord
    {
        public PLR() : base(RecordTypes.PLR, "Pin List Record") 
        {
            GRP_CNT = 1;
        }

        [STDF]
        public ushort GRP_CNT
        {
            get => (ushort)(GRP_INDX?.Length ?? 0);
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("GRP_CNT", "Group Count must be > 0");
                }
                GRP_INDX = new ushort[value];
                GRP_MODE = new ushort[value];
                GRP_RADX = new byte[value];
                PGM_CHAR = new string[value];
                PGM_CHAL = new string[value];
                RTN_CHAR = new string[value];
                RTN_CHAL = new string[value];
                Array.Fill(GRP_INDX, (ushort)0);
                Array.Fill(GRP_MODE, (ushort)0);
                Array.Fill(GRP_RADX, (byte)0);
                Array.Fill(PGM_CHAR, "");
                Array.Fill(PGM_CHAL, "");
                Array.Fill(RTN_CHAR, "");
                Array.Fill(RTN_CHAL, "");
            }
        }

        [STDF] public ushort[] GRP_INDX { get; set ; } = null;
        [STDF] public ushort[] GRP_MODE { get; set; } = null;
        [STDF] public byte[] GRP_RADX { get; set; } = null;
        [STDF] public string[] PGM_CHAR { get; set; } = null;
        [STDF] public string[] RTN_CHAR { get; set; } = null;
        [STDF] public string[] PGM_CHAL { get; set; } = null;
        [STDF] public string[] RTN_CHAL { get; set; } = null;
    }
}
