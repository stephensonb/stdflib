namespace STDFLib
{
    /// <summary>
    /// Part Results Record
    /// </summary>
    public class PRR : STDFRecord
    {
        public PRR() : base(RecordTypes.PRR) { }

        [STDF] public byte HEAD_NUM = 1;
        [STDF] public byte SITE_NUM = 1;
        [STDF] public byte PART_FLG = 0;
        [STDF] public ushort NUM_TEST;
        [STDF] public ushort HARD_BIN;
        [STDF] public ushort SOFT_BIN = 0xFFFF;
        [STDF] public short X_COORD = -32768;
        [STDF] public short Y_COORD = -32768;
        [STDF] public uint? TEST_T;
        [STDF] public string PART_ID = "";
        [STDF] public string PART_TXT = "";
        [STDF] public ByteArray PART_FIX;

        public override string ToString()
        {
            return string.Format("** Part Result {0},{1},{2},{3}", HEAD_NUM, SITE_NUM, PART_ID, PART_TXT);
        }
    }
}
