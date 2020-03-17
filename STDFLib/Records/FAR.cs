namespace STDFLib
{
    /// <summary>
    /// File Attributes Record
    /// </summary>
    public class FAR : STDFRecord
    {
        protected override RecordType TypeCode => 0x0010;

        [STDF(Order = 1)]
        public byte CPU_TYPE { get; set; } = (byte)STDFCpuTypes.i386;  // Default Intel CPU

        [STDF(Order = 2)]
        public byte STDF_VER { get; set; } = (byte)STDFVersions.STDFVer4; // Version 4 of the standard

        public override string Description => "File Attributes Record";
    }
}
