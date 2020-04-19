using System;

namespace STDFLib2
{
    [Flags]
    public enum PTROptionalData
    {
        RES_SCAL_Invalid = 0x01,  // Bit 0
        NoLoLimitSpec = 0x04,  // Bit 2
        NoHiLimitSpec = 0x08,  // Bit 3 
        LO_LIMIT_LLM_SCAL_Invalid = 0x10,  // Bit 4
        HI_LIMIT_HLM_SCAL_Invalid = 0x20,  // Bit 5 
        NoLoLimitThisTest = 0x40, // Bit 6
        NoHiLimitThisTest = 0x80, // Bit 7
        AllOptionalDataValid = 0x02, // Bit 1
        AllOptionalDataInvalid = 0xFF
    }
}
