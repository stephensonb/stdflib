using System;

namespace STDFLib2
{
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
