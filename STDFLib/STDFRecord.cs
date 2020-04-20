namespace STDFLib
{
    /// <summary>
    /// Abstract base class for STDF record classes.
    /// </summary>
    public abstract class STDFRecord : ISTDFRecord
    {
        /// <summary>
        /// Length of the record in the file on disk in byte.
        /// </summary>
        public ushort RecordLength { get; set; }
        /// <summary>
        /// Record type code, two bytes.  Byte 1 is the record type, byte 2 is the record sub type.
        /// </summary>
        public ushort RecordType { get; protected set; }

        protected STDFRecord(RecordTypes recordTypeCode)
        {
            RecordType = (ushort)recordTypeCode;
        }
    }
}
