using System;

namespace STDFLib
{
    /// <summary>
    /// Test Synopsis Record
    /// </summary>
    public class TSR : STDFRecord
    {
        private char _test_typ = TestTypes.Unknown;

        public override RecordType TypeCode => 0x0A1E;

        [STDF(Order = 1)]
        public byte HEAD_NUM { get; set; } = 0xFF;

        [STDF(Order = 2)]
        public byte SITE_NUM { get; set; } = 0x01;

        [STDF(Order = 3)]
        public char TEST_TYP
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

        [STDF(Order = 4)]
        public uint TEST_NUM { get; set; } = 0;

        [STDF(Order = 5)]
        public uint EXEC_CNT { get; set; } = 0xFFFFFFFF;

        [STDF(Order = 6)]
        public uint FAIL_CNT { get; set; } = 0xFFFFFFFF;

        [STDF(Order = 7)]
        public uint ALRM_CNT { get; set; } = 0xFFFFFFFF;

        [STDF(Order = 8)]
        public string TEST_NAM { get; set; } = "";

        [STDF(Order = 9)]
        public string SEQ_NAM { get; set; } = "";

        [STDF(Order = 10)]
        public string TEST_LBL { get; set; } = "";

        [STDF(Order = 11)]
        public byte OPT_FLAG { get; set; } = (byte)TSROptionalData.AllOptionalDataIsInvalid;

        [STDF(Order = 12)]
        public float TEST_TIM { get; set; } = 0;

        [STDF(Order = 13)]
        public float TEST_MIN { get; set; } = 0;

        [STDF(Order = 14)]
        public float TEST_MAX { get; set; } = 0;

        [STDF(Order = 15)]
        public float TST_SUMS { get; set; } = 0;

        [STDF(Order = 16)]
        public float TST_SQRS { get; set; } = 0;

        public override string Description => "Test Synopsis Record";

    }
}
