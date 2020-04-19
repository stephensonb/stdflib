using System;

namespace STDFLib2
{
    /// <summary>
    /// Audit Trail Record Definition
    /// </summary>
    public class ATR : STDFRecord
    {
        public ATR() : base((ushort)RecordTypes.ATR) { }

        [STDF] public DateTime MOD_TIM;
        [STDF] public string CMD_LINE = "";
    }
}
