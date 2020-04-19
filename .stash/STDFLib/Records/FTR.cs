using System.Linq;

namespace STDFLib2.Serialization
{
    /// <summary>
    /// Functional Test Record
    /// </summary>
    [STDFDefaults]
    public class FTR : STDFRecord, ITestResult, IHasDefaults
    {
        public FTR() : base(RecordTypes.FTR, "Functional Test Record") { }

        public int FailPinByteCount(int numPins) 
        {
            return numPins > 0 ? (numPins - 1) / 8 + 1 : 0;
        }

        [STDF] public uint TEST_NUM { get; set; } = 0x00;
        [STDF] public byte HEAD_NUM { get; set; } = 0x00;
        [STDF] public byte SITE_NUM { get; set; } = 0x00;
        [STDF] public byte TEST_FLG { get; set; } = 0x00;
        [STDFOptional] public byte OPT_FLAG { get; set; } = (byte)FTROptionalData.AllOptionalDataIsInvalid;
        [STDFOptional] public uint? CYCL_CNT { get; set; } = null;
        [STDFOptional] public uint? REL_VADR { get; set; } = null;
        [STDFOptional] public uint? REPT_CNT { get; set; } = null;
        [STDFOptional] public uint? NUM_FAIL { get; set; } = null;
        [STDFOptional] public int? XFAIL_AD { get; set; } = null;
        [STDFOptional] public int? YFAIL_AD { get; set; } = null;
        [STDFOptional] public short? VECT_OFF { get; set; } = null;
        [STDFOptional] public ushort RTN_ICNT { get => (ushort)(RTN_INDX?.Length ?? 0); set { RTN_INDX = new ushort[value]; RTN_STAT = new byte[value]; } }
        [STDFOptional] public ushort PGM_ICNT { get => (ushort)(PGM_INDX?.Length ?? 0); set { PGM_INDX = new ushort[value]; PGM_STAT = new byte[value]; } }
        [STDFOptional] public ushort[] RTN_INDX { get; set; } = new ushort[0];
        [STDFOptional] public byte[] RTN_STAT { get; set; } = new byte[0];
        [STDFOptional] public ushort[] PGM_INDX { get; set; } = new ushort[0];
        [STDFOptional] public byte[] PGM_STAT { get; set; } = new byte[0];
        [STDFOptional] public ushort RSLT_PIN_CNT { get => (ushort)(FAIL_PIN?.Length * 8 ?? 0); set => FAIL_PIN = new byte[FailPinByteCount(value)]; }
        [STDFOptional] public byte[] FAIL_PIN { get; set; } = new byte[0];
        [STDFOptional] public string VECT_NAM { get; set; } = "";
        [STDFOptional] public string TIME_SET { get; set; } = "";
        [STDFOptional] public string OP_CODE { get; set; } = "";
        [STDFOptional] public string TEST_TXT { get; set; } = "";
        [STDFOptional] public string ALARM_ID { get; set; } = "";
        [STDFOptional] public string PROG_TXT { get; set; } = "";
        [STDFOptional] public string RSLT_TXT { get; set; } = "";
        [STDFOptional] public byte PATG_NUM { get; set; } = 255;
        [STDFOptional] public byte SPIN_MAP_LEN { get => (byte)(SPIN_MAP?.Length ?? 0); set { SPIN_MAP = new byte[value]; } }
        [STDFOptional] public byte[] SPIN_MAP { get; set; } = new byte[0];

        public override string ToString()
        {
            return string.Format("** Test# {0},{1},{2},{3}", TEST_NUM, HEAD_NUM, SITE_NUM, "Functional");
        }
    }
}