using System;

namespace STDFLib
{
    /// <summary>
    /// Test Synopsis Record
    /// </summary>
    public class TSR : STDFRecord
    {
        private char _test_typ = TestTypes.Unknown;

        public TSR() : base(RecordTypes.TSR, "Test Synopsis Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0xFF;

        [STDF] public byte SITE_NUM { get; set; } = 0x01;

        [STDF] public char TEST_TYP
        {
            get => _test_typ;

            set
            {
                switch(char.ToUpper(value))
                {
                    case TestTypes.Unknown:
                    case TestTypes.Functional:
                    case TestTypes.MultipleResultParametric:
                    case TestTypes.Parametric:
                        _test_typ = char.ToUpper(value);
                        break;
                    default:
                        throw new ArgumentException(string.Format("Unsupported Test Type code (TEST_TYP).  Valid values are P, F, M or space.  Recieved {0}.", value));
                }
            }
        }

        [STDF] public uint TEST_NUM { get; set; } = 0;

        [STDF] public uint EXEC_CNT { get; set; } = 0xFFFFFFFF;

        [STDF] public uint FAIL_CNT { get; set; } = 0xFFFFFFFF;

        [STDF] public uint ALRM_CNT { get; set; } = 0xFFFFFFFF;

        [STDF] public string TEST_NAM { get; set; } = "";

        [STDF] public string SEQ_NAM { get; set; } = "";

        [STDF] public string TEST_LBL { get; set; } = "";

        [STDF] public byte OPT_FLAG { get; set; } = (byte)TSROptionalData.AllOptionalDataIsInvalid;

        [STDF] public float TEST_TIM { get; set; } = 0;

        [STDF] public float TEST_MIN { get; set; } = 0;

        [STDF] public float TEST_MAX { get; set; } = 0;

        [STDF] public float TST_SUMS { get; set; } = 0;

        [STDF] public float TST_SQRS { get; set; } = 0;
    }
}
