namespace STDFLib2
{
    /// <summary>
    /// Public interface for STDF Records
    /// </summary>
    public interface ISTDFRecord
    {
        uint Length { get; set; }
        RecordTypes RecordType { get; set; }
        STDFVersions Version { get; }
    }

    public interface IHasDefaults
    {

    }
}

