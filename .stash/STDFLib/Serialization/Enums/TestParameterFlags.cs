using System;

namespace STDFLib2
{
    [Flags]
    public enum TestParameterFlags
    {
        AllBitsZero = 0x00,
        ScaleError = 0x01,
        DriftError = 0x02,
        OscillationDetected = 0x04,
        AboveHiLimit = 0x08,
        BelowLoLimit = 0x10,
        PassedAltLimits = 0x20,
        LoLimitInclusive = 0x40,
        HiLimitInclusive = 0x80
    }
}
