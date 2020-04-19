using System.Collections.Generic;
using System.Linq;

namespace STDFLib2
{
    /// <summary>
    /// Generic Data Record
    /// </summary>
    public class GDR : STDFRecord
    {
        public GDR() : base((ushort)RecordTypes.GDR) { }

        [STDF] public ushort FLD_CNT = 0;
        [STDF("FLD_CNT")] public VarDataField[] GEN_DATA;

        public override string ToString()
        {
            return string.Format("GDR: {0}", string.Join(' ',GEN_DATA.Select(x => string.Format("{0}", x?.Value ?? ""))));
        }
    }
}
