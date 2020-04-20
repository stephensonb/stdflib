namespace STDFLib
{
    /// <summary>
    /// File Attributes Record
    /// </summary>
    public class FAR : STDFRecord
    {
        public FAR() : base(RecordTypes.FAR)
        {
        }

        [STDF] public byte CPU_TYPE = 0;  // Default Intel CPU
        [STDF] public byte STDF_VER = 0;
    }
}
