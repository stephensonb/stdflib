using System.Collections.Generic;

namespace STDFLib
{
    /// <summary>
    /// Functional Test Record
    /// </summary>
    public class FTR : STDFRecord
    {
        private List<ushort> _rtn_indx = new List<ushort>();
        private Nibbles _rtn_stat = new Nibbles();
        private List<ushort> _pgm_indx = new List<ushort>();
        private Nibbles _pgm_stat = new Nibbles();

        public FTR() : base(0x0F14, "Functional Test Record") { }

        public override int GetItemCount(string propertyName)
        {
            return propertyName switch
            {
                "RTN_ICNT" => RTN_ICNT,
                "PGM_ICNT" => PGM_ICNT,
                _ => base.GetItemCount(propertyName),
            };
        }

        [STDF] public uint TEST_NUM { get; set; } = 0xFFFFFFFF;

        [STDF] public byte HEAD_NUM { get; set; } = 0x01;

        [STDF] public byte SITE_NUM { get; set; } = 0x01;

        [STDF] public byte TEST_FLG { get; set; } = (byte)TestResultFlags.InvalidResult;

        [STDF] public byte OPT_FLG { get; set; } = (byte)FTROptionalData.AllOptionalDataIsInvalid;

        [STDF] public uint CYCL_CNT { get; set; } = 0;

        [STDF] public uint REL_VADR { get; set; } = 0;

        [STDF] public uint REPT_CNT { get; set; } = 0;

        [STDF] public uint NUM_FAIL { get; set; } = 0;

        [STDF] public int XFAIL_AD { get; set; } = 0;

        [STDF] public int YFAIL_AD { get; set; } = 0;

        [STDF] public short VECT_OFF { get; set; } = 0;

        [STDF] public ushort RTN_ICNT
        {
            get => (ushort)RTN_INDX.Length;

            set
            {
                RTN_INDX = new ushort[value];
                RTN_STAT.Clear();
                RTN_STAT.AddRange(new byte[value]);
            }
        }

        [STDF] public ushort PGM_ICNT
        {
            get => (ushort)PGM_INDX.Length;

            set
            {
                PGM_INDX = new ushort[value];
                PGM_STAT.Clear();
                PGM_STAT.AddRange(new byte[value]);
            }
        }


        [STDF("RTN_ICNT")] public ushort[] RTN_INDX
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

        [STDF("RTN_ICNT")] public Nibbles RTN_STAT 
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

        [STDF("PGM_ICNT")] public ushort[] PGM_INDX
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

        [STDF("PGM_ICNT")] public Nibbles PGM_STAT 
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

        [STDF] public BitField2 FAIL_PIN { get; set; } = new BitField2();

        [STDF] public string VECT_NAM { get; set; } = "";

        [STDF] public string TIME_SET { get; set; } = "";

        [STDF] public string OP_CODE { get; set; } = "";

        [STDF] public string TEST_TXT { get; set; } = "";

        [STDF] public string ALARM_ID { get; set; } = "";

        [STDF] public string PROG_TXT { get; set; } = "";

        [STDF] public string RSLT_TXT { get; set; } = "";

        [STDF] public byte PATG_NUM { get; set; } = 0;

        [STDF] public BitField2 SPIN_MAP { get; set; } = new BitField2();

    }
}
