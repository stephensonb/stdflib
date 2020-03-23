using System.Runtime.Serialization;

namespace STDFLib
{
    /// <summary>
    /// Public interface for STDF Records
    /// </summary>
    public interface ISTDFRecord
    {
        STDFVersions Version { get; }
        RecordType RecordType { get; }
        int GetItemCount(string itemCountProperty);
    }
}

