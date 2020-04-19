namespace STDFLib2
{
    /// <summary>
    /// Public interface for STDF Records
    /// </summary>
    public interface ISTDFRecord
    {
        ushort RecordLength { get; set; }
        ushort RecordType { get; }
        bool SupportsDefaults { get; }
    }
}

