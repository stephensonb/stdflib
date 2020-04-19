namespace STDFLib2
{
    /// <summary>
    /// File Attributes Record
    /// </summary>
    public class FAR : STDFRecord
    {
        public FAR() : base((ushort)RecordTypes.FAR) 
        {
            //RecordType = (ushort)RecordTypes.FAR;
        }

        [STDF] public byte CPU_TYPE = 0;  // Default Intel CPU
        [STDF] public byte STDF_VER = 0;
    }
}
