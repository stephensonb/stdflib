using System;

namespace STDFLib
{
    /// <summary>
    /// Master Information record
    /// </summary>
    public class MIR : STDFRecord
    {
        private char _prot_cod = ' ';
        private char _rtst_cod = ' ';
        private char _cmod_cod = ' ';
        private char _mode_cod = ' ';

        protected override RecordType TypeCode => 0x0110;

        [STDF(Order = 1)]
        public DateTime SETUP_T { get; set; } = DateTime.Now;

        [STDF(Order = 2)]
        public DateTime START_T { get; set; } = DateTime.Now;

        [STDF(Order = 3)]
        public byte STAT_NUM { get; set; } = 1;

        [STDF(Order = 4)]
        public char MODE_COD
        {
            get => _mode_cod;

            set
            {
                if (!char.IsLetterOrDigit(value) && value != ' ')
                {
                    throw new ArgumentException(string.Format("Unsupported Test Mode Code (MODE_COD) value.  Valid values are 0-9, A-Z or a space.  Value received was '{0}'.", value));
                }
                _mode_cod = value;
            }
        }

        [STDF(Order = 5)]
        public char RTST_COD 
        {
            get => _rtst_cod;

            set
            {
                if (!char.IsLetterOrDigit(value) && value != ' ')
                {
                    throw new ArgumentException(string.Format("Unsupported Retest Code (RTST_COD) value.  Valid values are 0-9, A-Z or a space.  Value received was '{0}'.", value));
                }
            _rtst_cod = value;
            }
        }

        [STDF(Order = 6)]
        public char PROT_COD
        {
            get => _prot_cod;

            set
            {
                if (!char.IsLetterOrDigit(value) && value != ' ')
                {
                    throw new ArgumentException(string.Format("Unsupported Protection Code (PROT_COD) value.  Valid values are 0-9, A-Z or a space.  Value received was '{0}'.", value));
                }
                _prot_cod = value;
            }
        }

        [STDF(Order = 7)]
        public ushort BURN_TIM { get; set; } = 65535;

        [STDF(Order = 8)]
        public char CMOD_COD
        {
            get => _cmod_cod;

            set
            {
                if (!char.IsLetterOrDigit(value) && value != ' ')
                {
                    throw new ArgumentException(string.Format("Unsupported Command Mode Code (CMOD_COD) value.  Valid values are 0-9, A-Z or a space.  Value received was '{0}'.", value));
                }
                _cmod_cod = value;
            }
        }

        [STDF(Order = 9)]
        public string LOT_ID { get; set; } = "";

        [STDF(Order = 10)]
        public string PART_TYP { get; set; } = "";

        [STDF(Order = 11)]
        public string NODE_NAM { get; set; } = "";

        [STDF(Order = 12)]
        public string TSTR_TYP { get; set; } = "";

        [STDF(Order = 13)]
        public string JOB_NAM { get; set; } = "";

        [STDF(Order = 14)]
        public string JOB_REV { get; set; } = "";

        [STDF(Order = 15)]
        public string SBLOT_ID { get; set; } = "";

        [STDF(Order = 16)]
        public string OPER_NAM { get; set; } = "";

        [STDF(Order = 17)]
        public string EXEC_TYP { get; set; } = "";

        [STDF(Order = 18)]
        public string EXEC_VER { get; set; } = "";

        [STDF(Order = 19)]
        public string TEST_COD { get; set; } = "";

        [STDF(Order = 20)]
        public string TST_TEMP { get; set; } = "";

        [STDF(Order = 21)]
        public string USER_TXT { get; set; } = "";

        [STDF(Order = 22)]
        public string AUX_FILE { get; set; } = "";

        [STDF(Order = 23)]
        public string PKG_TYP { get; set; } = "";

        [STDF(Order = 24)]
        public string FAMLY_ID { get; set; } = "";

        [STDF(Order = 25)]
        public string DATE_COD { get; set; } = "";

        [STDF(Order = 26)]
        public string FACIL_ID { get; set; } = "";

        [STDF(Order = 27)] 
        public string FLOOR_ID { get; set; } = "";

        [STDF(Order = 28)]
        public string PROC_ID { get; set; } = "";

        [STDF(Order = 29)]
        public string OPER_FRQ { get; set; } = "";

        [STDF(Order = 30)]
        public string SPEC_NAM { get; set; } = "";

        [STDF(Order = 31)]
        public string SPEC_VER { get; set; } = "";

        [STDF(Order = 32)]
        public string FLOW_ID { get; set; } = "";

        [STDF(Order = 33)]
        public string SETUP_ID { get; set; } = "";

        [STDF(Order = 34)]
        public string DSGN_REV { get; set; } = "";

        [STDF(Order = 35)]
        public string ENG_ID { get; set; } = "";

        [STDF(Order = 36)]
        public string ROM_COD { get; set; } = "";

        [STDF(Order = 37)]
        public string SERL_NUM { get; set; } = "";

        [STDF(Order = 38)]
        public string SUPR_NAM { get; set; } = "";

        public override string Description => "Master Information Record";
    }
}
