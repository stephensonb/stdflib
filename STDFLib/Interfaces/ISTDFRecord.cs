namespace STDFLib
{
    /// <summary>
    /// Public interface for STDF Records
    /// </summary>
    public interface ISTDFRecord
    {
        /// <summary>
        /// 
        /// </summary>
        ushort RecordLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        ushort RecordType { get; }
    }
}

