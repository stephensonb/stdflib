using System;

namespace STDFLib
{
    [Flags]
    public enum TestResultFlags
    {
        AlarmDetected = 0x01,
        InvalidResult = 0x02,
        DataUnreliable = 0x04,
        TimeoutOccured = 0x08,
        TestExecuted = 0x10,
        TestAborted = 0x20,
        PassFailUnknown = 0x40,
        TestFailed = 0x80
    }
}
