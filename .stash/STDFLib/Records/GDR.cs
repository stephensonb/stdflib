using System.Collections.Generic;
using System.Linq;

namespace STDFLib2
{
    /// <summary>
    /// Generic Data Record
    /// </summary>
    public class GDR : STDFRecord
    {
        public GDR() : base(RecordTypes.GDR, "Generic DataRecord") { }

        [STDF] public ushort FLD_CNT { get => (ushort)(FieldData?.Length ?? 0); set => FieldData = new VarDataField[value]; }
        [STDF] public VarDataField[] FieldData { get; set; } = new VarDataField[0];
        public override string ToString()
        {
            return string.Format("GDR: {0}", string.Join(' ',FieldData.Select(x => string.Format("{0}", x.Value))));
        }
    }
}
