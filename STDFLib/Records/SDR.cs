namespace STDFLib
{
    /// <summary>
    /// Site Description Record
    /// </summary>
    public class SDR : STDFRecord
    {
        public SDR() : base(RecordTypes.SDR, "Site Description Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0x01;

        // Site Group must be unique within an STDF file
        [STDF] public byte SITE_GRP { get; set; } = 0x01;

        [STDF] public byte SITE_CNT
        {
            get => (byte)SITE_NUM.Length;

            set
            {
                SITE_NUM = new byte[value];
            }
        }

        [STDF] public byte[] SITE_NUM { get; set; } = new byte[] { };

        [STDF] public string HAND_TYP { get; set; } = "";

        [STDF] public string HAND_ID { get; set; } = "";

        [STDF] public string CARD_TYP { get; set; } = "";

        [STDF] public string CARD_ID { get; set; } = "";

        [STDF] public string LOAD_TYP { get; set; } = "";

        [STDF] public string LOAD_ID { get; set; } = "";

        [STDF] public string DIB_TYP { get; set; } = "";

        [STDF] public string DIB_ID { get; set; } = "";

        [STDF] public string CABL_TYP { get; set; } = "";

        [STDF] public string CABL_ID { get; set; } = "";

        [STDF] public string CONT_TYP { get; set; } = "";

        [STDF] public string CONT_ID { get; set; } = "";

        [STDF] public string LASR_TYP { get; set; } = "";

        [STDF] public string LASR_ID { get; set; } = "";

        [STDF] public string EXTR_TYP { get; set; } = "";

        [STDF] public string EXTR_ID { get; set; } = "";
    }
}
