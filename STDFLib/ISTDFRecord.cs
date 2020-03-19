namespace STDFLib
{
    /// <summary>
    /// Public interface for STDF Records
    /// </summary>
    public interface ISTDFRecord
    {
        // The version of the STDF specification that this record conforms to
        STDFVersions Version { get; }
        RecordType TypeCode { get; }
    }
}

