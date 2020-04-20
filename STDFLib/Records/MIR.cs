using System;

namespace STDFLib
{
    /// <summary>
    /// Master Information record
    /// </summary>
    public class MIR : STDFRecord
    {
        public MIR() : base(RecordTypes.MIR) { }

        [STDF] public DateTime SETUP_T;
        [STDF] public DateTime START_T;
        [STDF] public byte STAT_NUM = 0;
        [STDF] public char MODE_COD = ' ';
        [STDF] public char RTST_COD = ' ';
        [STDF] public char PROT_COD = ' ';
        [STDF] public ushort BURN_TIM = 65535;
        [STDF] public char CMOD_COD = ' ';
        [STDF] public string LOT_ID = "";
        [STDF] public string PART_TYP = "";
        [STDF] public string NODE_NAM = "";
        [STDF] public string TSTR_TYP = "";
        [STDF] public string JOB_NAM = "";
        [STDF] public string JOB_REV = "";
        [STDF] public string SBLOT_ID = "";
        [STDF] public string OPER_NAM = "";
        [STDF] public string EXEC_TYP = "";
        [STDF] public string EXEC_VER = "";
        [STDF] public string TEST_COD = "";
        [STDF] public string TST_TEMP = "";
        [STDF] public string USER_TXT = "";
        [STDF] public string AUX_FILE = "";
        [STDF] public string PKG_TYP = "";
        [STDF] public string FAMLY_ID = "";
        [STDF] public string DATE_COD = "";
        [STDF] public string FACIL_ID = "";
        [STDF] public string FLOOR_ID = "";
        [STDF] public string PROC_ID = "";
        [STDF] public string OPER_FRQ = "";
        [STDF] public string SPEC_NAM = "";
        [STDF] public string SPEC_VER = "";
        [STDF] public string FLOW_ID = "";
        [STDF] public string SETUP_ID = "";
        [STDF] public string DSGN_REV = "";
        [STDF] public string ENG_ID = "";
        [STDF] public string ROM_COD = "";
        [STDF] public string SERL_NUM = "";
        [STDF] public string SUPR_NAM = "";
    }
}
