using System;
using System.Collections.Generic;

namespace STDFLib
{
    /// <summary>
    /// Generic Data Record
    /// </summary>
    public class GDR : STDFRecord
    {
        private List<object> _gen_data { get; set; } = new List<object>();

        public GDR() : base(RecordTypes.GDR, "Generic DataRecord") { }

        public override int GetItemCount(string propertyName)
        {
            return propertyName switch
            {
                "FLD_CNT" => FLD_CNT,
                _ => base.GetItemCount(propertyName),
            };
        }

        [STDF] public ushort FLD_CNT { get; set; }

        [STDF("FLD_CNT")] public object[] FieldData
        {
            get
            {
                return _gen_data.ToArray();
            }

            set
            {
                if (value != null)
                {
                    _gen_data.AddRange(value);                
                }
            }
        }

        public void Add(object fieldValue)
        {
            int typeCode = VarData.GetFieldTypeCode(fieldValue.GetType());

            if (typeCode < 0)
            {
                throw new ArgumentException(string.Format("Trying to add an unsupported field type {0} to a generic data record.", fieldValue.GetType().Name));
            }

            _gen_data.Add(fieldValue);
        }
    }
}
