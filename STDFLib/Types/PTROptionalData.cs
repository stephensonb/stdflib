using System;

namespace STDFLib
{

    [Flags]
    public enum PTROptionalData
    {
        RES_SCAL_Invalid = 0x01 | AllOptionalDataValid,  // Bit 0
        NoLoLimitSpec = 0x04 | AllOptionalDataValid,  // Bit 2
        NoHiLimitSpec = 0x08 | AllOptionalDataValid,  // Bit 3 
        LO_LIMIT_LLM_SCAL_Invalid = 0x10 | AllOptionalDataValid,  // Bit 4
        HI_LIMIT_HLM_SCAL_Invalid = 0x20 | AllOptionalDataValid,  // Bit 5 
        NoLoLimitThisTest = 0x40 | AllOptionalDataValid, // Bit 6
        NoHiLimitThisTest = 0x80 | AllOptionalDataValid, // Bit 7
        AllOptionalDataValid = 0x02, // Bit 1
        AllOptionalDataInvalid = 0xFF // Bit 1
    }

    [Flags]
    public enum MPROptionalData
    {
        RES_SCAL_Invalid = 0x01 | AllOptionalDataValid,  // Bit 0
        NoLoLimitSpec = 0x04 | AllOptionalDataValid,  // Bit 2
        NoHiLimitSpec = 0x08 | AllOptionalDataValid,  // Bit 3 
        LO_LIMIT_LLM_SCAL_Invalid = 0x10 | AllOptionalDataValid,  // Bit 4
        HI_LIMIT_HLM_SCAL_Invalid = 0x20 | AllOptionalDataValid,  // Bit 5 
        NoLoLimitThisTest = 0x40 | AllOptionalDataValid, // Bit 6
        NoHiLimitThisTest = 0x80 | AllOptionalDataValid, // Bit 7
        AllOptionalDataValid = 0x02, // Bit 1
        AllOptionalDataInvalid = 0xFF // Bit 1
    }

    [Flags]
    public enum FTROptionalData
    {
        CycleCountDataIsInvalid = 0x01 | AllOptionalDataValid,  // Bit 0
        RelativeVectorAddressIsInvalid = 0x02 | AllOptionalDataValid,  // Bit 1
        RepeatCountOfVectorIsInvalid = 0x04 | AllOptionalDataValid,  // Bit 2
        NumberOfFailsIsInvalid = 0x08 | AllOptionalDataValid,  // Bit 3
        XYFailAddressIsInvalid = 0x10 | AllOptionalDataValid,  // Bit 4
        VectorOffsetDataIsInvalid = 0x20 | AllOptionalDataValid,  // Bit 5 
        AllOptionalDataValid = 0b11000000, // Bit 6 and 7 must be 1 
        AllOptionalDataIsInvalid = 0xFF
    }
}
