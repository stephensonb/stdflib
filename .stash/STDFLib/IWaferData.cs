using System.Collections.Generic;

namespace STDFLib2.Serialization
{
    public interface IWaferData
    {
        WIR Info { get; set; }
        List<IPartData> Parts { get; set; }
        WRR Results { get; set; }
    }
}
