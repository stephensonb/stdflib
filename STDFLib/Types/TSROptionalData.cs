using System;

namespace STDFLib
{
    [Flags]
    public enum TSROptionalData
    {
        TEST_MIN_Invalid = 0x01,  // Bit 0
        TEST_MAX_Invalid = 0x02,  // Bit 1
        TEST_TIM_Invalid = 0x04,  // Bit 2
        TST_SUMS_Invalid = 0x10,  // Bit 4 
        TST_SQRS_Invalid = 0x20,  // Bit 5 
        AllOptionalDataIsValid = 0x08 | 0x40 | 0x80, // All bits zero except 3, 6 and 7, which are 1 per spec.
        AllOptionalDataIsInvalid = 0xFF
    }
}
