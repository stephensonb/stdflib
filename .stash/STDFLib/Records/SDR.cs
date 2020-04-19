namespace STDFLib2
{
    /// <summary>
    /// Site Description Record
    /// </summary>
    public class SDR : STDFRecord
    {
        public SDR() : base(RecordTypes.SDR, "Site Description Record") { }

        [STDF] public byte HEAD_NUM { get; set; } = 0x00;
        // Site Group must be unique within an STDF file
        [STDF] public byte SITE_GRP { get; set; } = 0x00;
        [STDF] public byte SITE_CNT { get => (byte)(SITE_NUM?.Length ?? 0); set => SITE_NUM = new byte[value]; }
        [STDF] public byte[] SITE_NUM { get; set; } = new byte[0];
        [STDFOptional] public string HAND_TYP { get; set; } = "";
        [STDFOptional] public string HAND_ID { get; set; } = "";
        [STDFOptional] public string CARD_TYP { get; set; } = "";
        [STDFOptional] public string CARD_ID { get; set; } = "";
        [STDFOptional] public string LOAD_TYP { get; set; } = "";
        [STDFOptional] public string LOAD_ID { get; set; } = "";
        [STDFOptional] public string DIB_TYP { get; set; } = "";
        [STDFOptional] public string DIB_ID { get; set; } = "";
        [STDFOptional] public string CABL_TYP { get; set; } = "";
        [STDFOptional] public string CABL_ID { get; set; } = "";
        [STDFOptional] public string CONT_TYP { get; set; } = "";
        [STDFOptional] public string CONT_ID { get; set; } = "";
        [STDFOptional] public string LASR_TYP { get; set; } = "";
        [STDFOptional] public string LASR_ID { get; set; } = "";
        [STDFOptional] public string EXTR_TYP { get; set; } = "";
        [STDFOptional] public string EXTR_ID { get; set; } = "";
    }
}
