using System;

namespace STDFLib2
{
    /// <summary>
    /// Audit Trail Record Definition
    /// </summary>
    public class ATR : STDFRecord
    {
        public ATR() : base(RecordTypes.ATR, "Audit Trail Record") { }

        [STDF] public DateTime MOD_TIM { get; set; } = DateTime.Now;
        [STDF] public string CMD_LINE { get; set; } = "";
    }
}
