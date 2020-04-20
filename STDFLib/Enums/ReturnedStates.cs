namespace STDFLib.Serialization
{
    public enum ReturnedStates
    {
        Low = 0x00,
        High = 0x01,
        Midband = 0x02,
        Glitch = 0x03,
        Undetermined = 0x04,
        FailedLow = 0x05,
        FailedHigh = 0x06,
        FailedMidband = 0x07,
        FailedWithGlitch = 0x08,
        Open = 0x09,
        Short = 0x0A
    }
}
