using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace STDFLib
{
    public abstract class STDFRecord : ISTDFRecord
    {
        public static Dictionary<object, ISTDFRecord> RecordDefaults = new Dictionary<object, ISTDFRecord>();

        // This base class is for records conforming to Version 4.0 of the STDF specification.
        public STDFVersions Version { get; } = STDFVersions.STDFVer4;

        // public fields
        public RecordType RecordType { get; } = 0;
        public string Description { get; } = "";

        public STDFRecord()
        {
        }

        public STDFRecord(RecordType recordType, string description)
        {
            RecordType = recordType;
            Description = description;
        }

        public virtual int GetItemCount(string propertyName)
        {
            switch (propertyName)
            {
                //              case "prop1": return prop1;
                //              case "prop2": return prop2;
                default: return -1;
            }
        }
    }
}

