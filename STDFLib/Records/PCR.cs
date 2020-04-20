namespace STDFLib
{
    /// <summary>
    /// Part Count Record
    /// </summary>
    public class PCR : STDFRecord
    {
        public PCR() : base(RecordTypes.PCR) { }

        [STDF] public byte HEAD_NUM = byte.MaxValue;
        [STDF] public byte SITE_NUM = 1;
        [STDF] public uint PART_CNT;
        [STDF] public uint RTST_CNT = uint.MaxValue;
        [STDF] public uint ABRT_CNT = uint.MaxValue;
        [STDF] public uint GOOD_CNT = uint.MaxValue;
        [STDF] public uint FUNC_CNT = uint.MaxValue;
    }
}
