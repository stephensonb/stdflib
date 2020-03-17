namespace STDFLib
{
    /// <summary>
    /// Public interface for STDF Records
    /// </summary>
    public interface ISTDFRecord
    {
        // The version of the STDF specification that this record conforms to
        STDFVersions Version { get; }

        // Length of the record (in bytes)
        uint REC_LEN { get; }

        // The major record type (type code)
        byte REC_TYP { get; }

        // The minor record type (sub code)
        byte REC_SUB { get; }
    }
}

