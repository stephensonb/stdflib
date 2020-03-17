using System;

namespace STDFLib
{
    /// <summary>
    /// Audit Trail Record Definition
    /// </summary>
    public class ATR : STDFRecord
    {
        protected override RecordType TypeCode => 0x0020;

        [STDF(Order = 1)]
        public DateTime MOD_TIM { get; set; } = DateTime.Now;

        [STDF(Order = 2)]
        public string CMD_LINE { get; set; } = "";

        public override string Description => "Audit Trail Record";
    }
}
