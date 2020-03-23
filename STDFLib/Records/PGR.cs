using System;
using System.Collections.Generic;
using System.Linq;

namespace STDFLib
{
    /// <summary>
    /// Pin Group Record
    /// </summary>
    public class PGR : STDFRecord
    {
        private ushort _grp_indx = 32768;
        private ushort _indx_cnt = 0;
        private List<ushort> _pmr_indx = new List<ushort>();

        public PGR() : base(RecordTypes.PGR, "Pin Group Record") { }

        public override int GetItemCount(string propertyName)
        {
            return propertyName switch
            {
                "INDX_CNT" => INDX_CNT,
                _ => base.GetItemCount(propertyName)
            };
        }

        [STDF] public ushort GRP_INDX
        {
            get => _grp_indx;

            set
            {
                if (value < 32768)
                {
                    throw new ArgumentException(string.Format("Invalid Pin Group Index.  Value must be between 32,768 and 65,535.  Value received was {0}.", value));
                }
            }
        }

        [STDF] public string GRP_NAM { get; set; } = "";

        [STDF] public ushort INDX_CNT
        {
            get => (ushort)_pmr_indx.Count;

            set
            {
                _indx_cnt = value;
                PMR_INDX = new ushort[value];
            }
        }

        [STDF("INDX_CNT")]
        public ushort[] PMR_INDX 
        {
            get
            {
                return _pmr_indx.ToArray();
            }

            set
            {
                _pmr_indx.Clear();
                _pmr_indx.AddRange(value);
            } 
        }
    }
}
