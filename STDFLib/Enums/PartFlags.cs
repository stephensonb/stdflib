using System;

namespace STDFLib
{
    [Flags]
    public enum PartFlags
    {
        RetestPartID = 0x01,
        RetestXYCoord = 0x02,
        AbnormalEnd = 0x04,
        PartFailed = 0x08,
        PassFailInvalid = 0x10,
    }
}
