using System;

namespace STDFLib
{
    /// <summary>
    /// Part Results Record
    /// </summary>
    public class PRR : STDFRecord
    {
        private byte _prt_flg = 0;

        protected override RecordType TypeCode => 0x0520;

        [STDF(Order = 1)]
        public byte HEAD_NUM { get; set; } = 0x01;

        [STDF(Order = 2)]
        public byte SITE_NUM { get; set; } = 0x01;

        [STDF(Order = 3)]
        public byte PART_FLG
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

        [STDF(Order = 4)]
        public ushort NUM_TEST { get; set; } = 0;

        [STDF(Order = 5)]
        public ushort HARD_BIN { get; set; } = 0;

        [STDF(Order = 6)]
        public ushort SOFT_BIN { get; set; } = 0xFFFF;

        [STDF(Order = 7)]
        public short X_COORD { get; set; } = -32768;

        [STDF(Order = 8)]
        public short Y_COORD { get; set; } = -32768;

        [STDF(Order = 9)]
        public uint TEST_T { get; set; } = 0;

        [STDF(Order = 10)]
        public string PART_ID { get; set; } = "";

        [STDF(Order = 11)]
        public string PART_TXT { get; set; } = "";

        [STDF(Order = 12)]
        public BitField PART_FIX { get; set; } = new BitField();

        public override string Description => "Part Results Record";

    }
}
