using System.Collections.Generic;

namespace STDFLib
{
    /// <summary>
    /// Functional Test Record
    /// </summary>
    public class FTR : STDFRecord
    {
        private ushort _rtn_icnt = 0;
        private ushort _pgm_icnt = 0;
        private List<ushort> _rtn_indx = new List<ushort>();
        private Nibbles _rtn_stat = new Nibbles();
        private List<ushort> _pgm_indx = new List<ushort>();
        private Nibbles _pgm_stat = new Nibbles();

        public override RecordType TypeCode => 0x0F14;

        [STDF(Order = 1)]
        public uint TEST_NUM { get; set; } = 0xFFFFFFFF;

        [STDF(Order = 2)]
        public byte HEAD_NUM { get; set; } = 0x01;

        [STDF(Order = 3)]
        public byte SITE_NUM { get; set; } = 0x01;

        [STDF(Order = 4)]
        public byte TEST_FLG { get; set; } = (byte)TestResultFlags.InvalidResult;

        [STDF(Order = 5)]
        public byte OPT_FLG { get; set; } = (byte)FTROptionalData.AllOptionalDataIsInvalid;

        [STDF(Order = 6)]
        public uint CYCL_CNT { get; set; } = 0;

        [STDF(Order = 7)]
        public uint REL_VADR { get; set; } = 0;

        [STDF(Order = 8)]
        public uint REPT_CNT { get; set; } = 0;

        [STDF(Order = 9)]
        public uint NUM_FAIL { get; set; } = 0;

        [STDF(Order = 10)]
        public int XFAIL_AD { get; set; } = 0;

        [STDF(Order = 11)]
        public int YFAIL_AD { get; set; } = 0;

        [STDF(Order = 12)]
        public short VECT_OFF { get; set; } = 0;

        [STDF(Order = 13)]
        public ushort RTN_ICNT
        {
            get => (ushort)RTN_INDX.Length;

            set
            {
                _rtn_icnt = value;
                RTN_INDX = new ushort[value];
                RTN_STAT.Clear();
                RTN_STAT.AddRange(new byte[value]);
            }
        }

        [STDF(Order = 14)]
        public ushort PGM_ICNT
        {
            get => (ushort)PGM_INDX.Length;

            set
            {
                _pgm_icnt = value;
                PGM_INDX = new ushort[value];
                PGM_STAT.Clear();
                PGM_STAT.AddRange(new byte[value]);
            }
        }

        [STDF(Order = 15, ItemCountProvider = "RTN_ICNT")]
        public ushort[] RTN_INDX 
        {
            get
            {
                return _rtn_indx.ToArray();
            } 

            set
            {
                _rtn_indx.Clear();
                _rtn_indx.AddRange(value);
            }
        }

        [STDF(Order = 16, ItemCountProvider = "RTN_ICNT")]
        public Nibbles RTN_STAT 
        { 
            get
            {
                return _rtn_stat;
            } 

            set
            {
                _rtn_stat.Clear();
                _rtn_stat = value;
            } 
        }

        [STDF(Order = 17, ItemCountProvider = "PGM_ICNT")]
        public ushort[] PGM_INDX
        {
            get
            {
                return _pgm_indx.ToArray();
            }

            set
            {
                _pgm_indx.Clear();
                _pgm_indx.AddRange(value);
            }
        }

        [STDF(Order = 18, ItemCountProvider = "PGM_ICNT")]
        public Nibbles PGM_STAT 
        {
            get
            {
                return _pgm_stat;
            }

            set
            {
                _pgm_stat.Clear();
                _pgm_stat = value;
            }
        }

        [STDF(Order = 19)]
        public BitField2 FAIL_PIN { get; set; } = new BitField2();

        [STDF(Order = 20)]
        public string VECT_NAM { get; set; } = "";

        [STDF(Order = 21)]
        public string TIME_SET { get; set; } = "";

        [STDF(Order = 22)]
        public string OP_CODE { get; set; } = "";

        [STDF(Order = 23)]
        public string TEST_TXT { get; set; } = "";

        [STDF(Order = 24)]
        public string ALARM_ID { get; set; } = "";

        [STDF(Order = 25)]
        public string PROG_TXT { get; set; } = "";

        [STDF(Order = 26)]
        public string RSLT_TXT { get; set; } = "";

        [STDF(Order = 27)]
        public byte PATG_NUM { get; set; } = 0;

        [STDF(Order = 28)]
        public BitField2 SPIN_MAP { get; set; } = new BitField2();

        public override string Description => "Functional Test Record";
    }
}
