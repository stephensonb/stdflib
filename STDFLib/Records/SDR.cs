namespace STDFLib
{
    /// <summary>
    /// Site Description Record
    /// </summary>
    public class SDR : STDFRecord
    {
        public SDR() : base(RecordTypes.SDR) { }

        // Site Group must be unique within an STDF file
        [STDF] public byte HEAD_NUM = 1;
        [STDF] public byte SITE_GRP = 1;
        [STDF] public byte SITE_CNT = 0;
        [STDF("SITE_CNT")] public byte[] SITE_NUM;
        [STDF] public string HAND_TYP = "";
        [STDF] public string HAND_ID = "";
        [STDF] public string CARD_TYP = "";
        [STDF] public string CARD_ID = "";
        [STDF] public string LOAD_TYP = "";
        [STDF] public string LOAD_ID = "";
        [STDF] public string DIB_TYP = "";
        [STDF] public string DIB_ID = "";
        [STDF] public string CABL_TYP = "";
        [STDF] public string CABL_ID = "";
        [STDF] public string CONT_TYP = "";
        [STDF] public string CONT_ID = "";
        [STDF] public string LASR_TYP = "";
        [STDF] public string LASR_ID = "";
        [STDF] public string EXTR_TYP = "";
        [STDF] public string EXTR_ID = "";
    }
}
