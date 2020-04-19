using System.Text;
using System.Collections;

namespace STDFLib2
{
    public class STDFRecord : ISTDFRecord
    {
        public long ObjectId { get; set; } = 0;

        public ushort RecordLength { get; set; }
        public ushort RecordType { get; protected set; }
        public bool SupportsDefaults { get; protected set; }
        
        protected STDFRecord(ushort recordTypeCode, bool supportsDefaults=false)
        {
            RecordType = recordTypeCode;
            SupportsDefaults = supportsDefaults;
        }
    }
}
