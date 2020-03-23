using System;

namespace STDFLib
{
    /// <summary>
    /// Part Results Record
    /// </summary>
    public class PRR : STDFRecord
    {
        private byte _prt_flg = 0;

        public PRR() : base(RecordTypes.PRR, "Part Results Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0x01;

        [STDF] public byte SITE_NUM { get; set; } = 0x01;

        [STDF] public byte PART_FLG
        {
            get => _prt_flg;

            set
            {
                // Test if both bits 0 and 1 are set (1)
                if ((value & 0b00000011) > 0)
                {
                    throw new ArgumentException("Retest flags for PART_ID and XCOORD/YCOORD cannot both be set (bits 0 and 1).");
                }

                if ((value & 0b11100000) > 0)
                {
                    throw new ArgumentException("Part flag bits 5 through 7 are reserved and must be set to 0.");
                }
            }
        }

        [STDF] public ushort NUM_TEST { get; set; } = 0;

        [STDF] public ushort HARD_BIN { get; set; } = 0;

        [STDF] public ushort SOFT_BIN { get; set; } = 0xFFFF;

        [STDF] public short X_COORD { get; set; } = -32768;

        [STDF] public short Y_COORD { get; set; } = -32768;

        [STDF] public uint TEST_T { get; set; } = 0;

        [STDF] public string PART_ID { get; set; } = "";

        [STDF] public string PART_TXT { get; set; } = "";

        [STDF] public BitField PART_FIX { get; set; } = new BitField();
    }
}
