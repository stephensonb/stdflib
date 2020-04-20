using System;

namespace STDFLib
{
    /// <summary>
    /// Audit Trail Record Definition
    /// </summary>
    public class ATR : STDFRecord
    {
        public ATR() : base(RecordTypes.ATR) { }

        [STDF] public DateTime MOD_TIM;
        [STDF] public string CMD_LINE = "";
    }
}
