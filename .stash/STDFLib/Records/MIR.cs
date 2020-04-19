using System;

namespace STDFLib2
{
    /// <summary>
    /// Master Information record
    /// </summary>
    public class MIR : STDFRecord
    {
        public MIR() : base(RecordTypes.MIR, "Master Information Record") { }

        [STDF] public DateTime SETUP_T { get; set; } = DateTime.UnixEpoch;
        [STDF] public DateTime START_T { get; set; } = DateTime.UnixEpoch;
        [STDF] public byte STAT_NUM { get; set; } = 0;
        [STDFOptional] public char MODE_COD { get; set; } = ' ';
        [STDFOptional] public char RTST_COD { get; set; } = ' ';
        [STDFOptional] public char PROT_COD { get; set; } = ' ';
        [STDFOptional] public ushort BURN_TIM { get; set; } = 65535;
        [STDFOptional] public char CMOD_COD { get; set; } = ' ';
        [STDFOptional] public string LOT_ID { get; set; } = "";
        [STDFOptional] public string PART_TYP { get; set; } = "";
        [STDFOptional] public string NODE_NAM { get; set; } = "";
        [STDFOptional] public string TSTR_TYP { get; set; } = "";
        [STDFOptional] public string JOB_NAM { get; set; } = "";
        [STDFOptional] public string JOB_REV { get; set; } = "";
        [STDFOptional] public string SBLOT_ID { get; set; } = "";
        [STDFOptional] public string OPER_NAM { get; set; } = "";
        [STDFOptional] public string EXEC_TYP { get; set; } = "";
        [STDFOptional] public string EXEC_VER { get; set; } = "";
        [STDFOptional] public string TEST_COD { get; set; } = "";
        [STDFOptional] public string TST_TEMP { get; set; } = "";
        [STDFOptional] public string USER_TXT { get; set; } = "";
        [STDFOptional] public string AUX_FILE { get; set; } = "";
        [STDFOptional] public string PKG_TYP { get; set; } = "";
        [STDFOptional] public string FAMLY_ID { get; set; } = "";
        [STDFOptional] public string DATE_COD { get; set; } = "";
        [STDFOptional] public string FACIL_ID { get; set; } = "";
        [STDFOptional] public string FLOOR_ID { get; set; } = "";
        [STDFOptional] public string PROC_ID { get; set; } = "";
        [STDFOptional] public string OPER_FRQ { get; set; } = "";
        [STDFOptional] public string SPEC_NAM { get; set; } = "";
        [STDFOptional] public string SPEC_VER { get; set; } = "";
        [STDFOptional] public string FLOW_ID { get; set; } = "";
        [STDFOptional] public string SETUP_ID { get; set; } = "";
        [STDFOptional] public string DSGN_REV { get; set; } = "";
        [STDFOptional] public string ENG_ID { get; set; } = "";
        [STDFOptional] public string ROM_COD { get; set; } = "";
        [STDFOptional] public string SERL_NUM { get; set; } = "";
        [STDFOptional] public string SUPR_NAM { get; set; } = "";
    }
}
