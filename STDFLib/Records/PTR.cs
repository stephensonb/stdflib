using System.Reflection;
using System.Collections.Generic;

namespace STDFLib
{
    /// <summary>
    /// Parametric Test Record
    /// </summary>
    public class PTR : STDFRecord
    {         
        public PTR() : base(RecordTypes.PTR, "Parametric Test Record") { }

        [STDF] public uint TEST_NUM { get; set; } = 0xFFFFFFFF;

        [STDF] public byte HEAD_NUM { get; set; } = 0x01;

        [STDF] public byte SITE_NUM { get; set; } = 0x01;

        [STDF] public byte TEST_FLG { get; set; } = 0;

        [STDF] public byte PARM_FLG { get; set; } = 0x00;

        [STDF] public float RESULT { get; set; } = 0;

        [STDF] public string TEST_TXT { get; set; } = "";

        [STDF] public string ALARM_ID { get; set; } = "";

        [STDF] public byte OPT_FLAG { get; set; } = (byte)PTROptionalData.AllOptionalDataInvalid;

        [STDF] public sbyte RES_SCAL { get; set; } = 0;

        [STDF] public sbyte LLM_SCAL { get; set; } = 0;

        [STDF] public sbyte HLM_SCAL { get; set; } = 0;

        [STDF] public float LO_LIMIT { get; set; } = 0;

        [STDF] public float HI_LIMIT { get; set; } = 0;

        [STDF] public string UNITS { get; set; } = "";

        [STDF] public string C_RESFMT { get; set; } = "";

        [STDF] public string C_LLMFMT { get; set; } = "";

        [STDF] public string C_HLMFMT { get; set; } = "";

        [STDF] public float LO_SPEC { get; set; } = 0;

        [STDF] public float HI_SPEC { get; set; } = 0;
    }
}
