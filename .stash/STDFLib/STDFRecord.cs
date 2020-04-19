namespace STDFLib2
{
    // This base class is for records conforming to Version 4.0 of the STDF specification.
    public abstract class STDFRecord : ISTDFRecord
    {
        public STDFRecord()
        {
        }

        public STDFRecord(RecordType recordType, string description)
        {
            RecordType = recordType;
            Description = description;
        }

        public STDFVersions Version { get; } = STDFVersions.STDFVer4;
        public string Description { get; } = "";

        
        [STDF] public RecordTypes RecordType { get; set; } = 0;
        [STDF] public uint Length { get; set; }

    }
}

