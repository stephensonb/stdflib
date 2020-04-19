namespace STDFLib2
{
    /// <summary>
    /// Part Results Record
    /// </summary>
    public class PRR : STDFRecord
    {
        public PRR() : base(RecordTypes.PRR, "Part Results Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0x00;
        [STDF] public byte SITE_NUM { get; set; } = 0x00;
        [STDF] public byte PART_FLG { get; set; } = 0;
        [STDF] public ushort NUM_TEST { get; set; } = 0;
        [STDF] public ushort HARD_BIN { get; set; } = 0;
        [STDFOptional] public ushort SOFT_BIN { get; set; } = 0xFFFF;
        [STDFOptional] public short X_COORD { get; set; } = -32768;
        [STDFOptional] public short Y_COORD { get; set; } = -32768;
        [STDFOptional] public uint TEST_T { get; set; } = 0;
        [STDFOptional] public string PART_ID { get; set; } = "";
        [STDFOptional] public string PART_TXT { get; set; } = "";
        [STDFOptional] public byte PART_FIX_CNT { get => (byte)(PART_FIX?.Length ?? 0); set => PART_FIX = new byte[value]; }
        [STDFOptional] public byte[] PART_FIX { get; set; } = new byte[0];

        public override string ToString()
        {
            return string.Format("** Part Result {0},{1},{2},{3}", HEAD_NUM,SITE_NUM,PART_ID,PART_TXT);
        }
    }
}
