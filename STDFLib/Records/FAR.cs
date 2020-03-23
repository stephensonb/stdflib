namespace STDFLib
{
    /// <summary>
    /// File Attributes Record
    /// </summary>
    public class FAR : STDFRecord
    {
        public FAR() : base(RecordTypes.FAR, "File Attribute Record") { }

        [STDF] public byte CPU_TYPE { get; set; } = (byte)STDFCpuTypes.i386;  // Default Intel CPU

        [STDF] public byte STDF_VER { get; set; } = (byte)STDFVersions.STDFVer4; // Version 4 of the standard
    }
}
