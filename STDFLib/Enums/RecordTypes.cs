namespace STDFLib
{
    public enum RecordTypes : ushort
    {
        FAR = 0x000A,
        ATR = 0x0014,
        MIR = 0x010A,
        MRR = 0x0114,
        PCR = 0x011E,
        HBR = 0x0128,
        SBR = 0x0132,
        PMR = 0x013C,
        PGR = 0x013E,
        PLR = 0x013F,
        RDR = 0x0146,
        SDR = 0x0150,
        WIR = 0x020A,
        WRR = 0x0214,
        WCR = 0x021E,
        PIR = 0x050A,
        PRR = 0x0514,
        TSR = 0x0A1E,
        PTR = 0x0F0A,
        MPR = 0x0F0F,
        FTR = 0x0F14,
        BPS = 0x140A,
        EPS = 0x1414,
        GDR = 0x320A,
        DTR = 0x321E
    }
}
