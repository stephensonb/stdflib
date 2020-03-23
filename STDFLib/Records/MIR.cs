using System;
using System.IO;

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

        public MIR() : base(0x010A, "Master Information Record") { }

        [STDF] public DateTime SETUP_T { get; set; } = DateTime.Now;

        [STDF] public DateTime START_T { get; set; } = DateTime.Now;

        [STDF] public byte STAT_NUM { get; set; } = 1;

        [STDF] public char MODE_COD
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

        [STDF] public char RTST_COD
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

        [STDF] public char PROT_COD
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

        [STDF] public ushort BURN_TIM { get; set; } = 65535;

        [STDF] public char CMOD_COD
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

        [STDF] public string LOT_ID { get; set; } = "";

        [STDF] public string PART_TYP { get; set; } = "";

        [STDF] public string NODE_NAM { get; set; } = "";
        
        [STDF] public string TSTR_TYP { get; set; } = "";
        
        [STDF] public string JOB_NAM { get; set; } = "";
        
        [STDF] public string JOB_REV { get; set; } = "";
        
        [STDF] public string SBLOT_ID { get; set; } = "";
        
        [STDF] public string OPER_NAM { get; set; } = "";
        
        [STDF] public string EXEC_TYP { get; set; } = "";
        
        [STDF] public string EXEC_VER { get; set; } = "";
        
        [STDF] public string TEST_COD { get; set; } = "";
        
        [STDF] public string TST_TEMP { get; set; } = "";
        
        [STDF] public string USER_TXT { get; set; } = "";
        
        [STDF] public string AUX_FILE { get; set; } = "";
        
        [STDF] public string PKG_TYP { get; set; } = "";
        
        [STDF] public string FAMLY_ID { get; set; } = "";
        
        [STDF] public string DATE_COD { get; set; } = "";
        
        [STDF] public string FACIL_ID { get; set; } = "";
        
        [STDF] public string FLOOR_ID { get; set; } = "";
        
        [STDF] public string PROC_ID { get; set; } = "";
        
        [STDF] public string OPER_FRQ { get; set; } = "";
        
        [STDF] public string SPEC_NAM { get; set; } = "";
        
        [STDF] public string SPEC_VER { get; set; } = "";
        
        [STDF] public string FLOW_ID { get; set; } = "";
        
        [STDF] public string SETUP_ID { get; set; } = "";
        
        [STDF] public string DSGN_REV { get; set; } = "";
        
        [STDF] public string ENG_ID { get; set; } = "";
        
        [STDF] public string ROM_COD { get; set; } = "";
        
        [STDF] public string SERL_NUM { get; set; } = "";
        
        [STDF] public string SUPR_NAM { get; set; } = "";

    }
}
