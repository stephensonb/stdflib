using System.Collections.Generic;

namespace STDFLib2.Serialization
{
    public class WaferData : IWaferData
    {
        public WIR Info { get; set; }
        public List<IPartData> Parts { get; set; } = new List<IPartData>();
        public WRR Results { get; set; }
    }
}
