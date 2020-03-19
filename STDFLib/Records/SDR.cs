namespace STDFLib
{
    /// <summary>
    /// Site Description Record
    /// </summary>
    public class SDR : STDFRecord
    {
        public override RecordType TypeCode => 0x0150;

        public byte HEAD_NUM { get; set; } = 0x01;

        // Site Group must be unique within an STDF file
        [STDF(Order = 2)]
        public byte SITE_GRP { get; set; } = 0x01;

        [STDF(Order = 3)]
        public byte SITE_CNT
        {
            get => (byte)SITE_NUM.Length;

            set
            {
                // Do nothing.  Here for serialization.
            }
        }

        [STDF(Order = 4)]
        public byte[] SITE_NUM { get; set; } = new byte[] { };

        [STDF(Order = 5)]
        public string HAND_TYP { get; set; } = "";

        [STDF(Order = 6)]
        public string HAND_ID { get; set; } = "";

        [STDF(Order = 7)]
        public string CARD_TYP { get; set; } = "";

        [STDF(Order = 8)]
        public string CARD_ID { get; set; } = "";

        [STDF(Order = 9)]
        public string LOAD_TYP { get; set; } = "";

        [STDF(Order = 10)]
        public string LOAD_ID { get; set; } = "";

        [STDF(Order = 11)]
        public string DIB_TYP { get; set; } = "";

        [STDF(Order = 12)]
        public string DIB_ID { get; set; } = "";

        [STDF(Order = 13)]
        public string CABL_TYP { get; set; } = "";

        [STDF(Order = 14)]
        public string CABL_ID { get; set; } = "";

        [STDF(Order = 15)]
        public string CONT_TYP { get; set; } = "";

        [STDF(Order = 16)]
        public string CONT_ID { get; set; } = "";

        [STDF(Order = 17)]
        public string LASR_TYP { get; set; } = "";

        [STDF(Order = 18)]
        public string LASR_ID { get; set; } = "";

        [STDF(Order = 19)]
        public string EXTR_TYP { get; set; } = "";

        [STDF(Order = 20)]
        public string EXTR_ID { get; set; } = "";

        public override string Description => "Site Description Record";

    }
}
