

namespace STDFLib.Serialization
{
    public enum DualDriveModeProgramStates
    {
        LowAtD2LowAtD1 = 0,
        LowAtD2HighAtD1 = 1,
        HighAtD2LowAtD1 = 2,
        HighAtD2HighAtD1 = 3,
        CompareLow = 4,
        CompareHigh = 5,
        CompareMidband = 6,
        DoNotCompare = 7
    }
}
