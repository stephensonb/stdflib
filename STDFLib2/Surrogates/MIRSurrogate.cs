using System;

namespace STDFLib2
{
    public class MIRSurrogate : Surrogate<MIR>
    {
        public override void GetObjectData(MIR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.SETUP_T);
            SerializeValue(1, obj.START_T);
            SerializeValue(2, obj.STAT_NUM);
            SerializeValue(3, obj.MODE_COD);
            SerializeValue(4, obj.RTST_COD);
            SerializeValue(5, obj.PROT_COD);
            SerializeValue(6, obj.BURN_TIM);
            SerializeValue(7, obj.CMOD_COD);
            SerializeValue(8, obj.LOT_ID);
            SerializeValue(9, obj.PART_TYP);
            SerializeValue(10, obj.NODE_NAM);
            SerializeValue(11, obj.TSTR_TYP);
            SerializeValue(12, obj.JOB_NAM);
            SerializeValue(13, obj.JOB_REV);
            SerializeValue(14, obj.SBLOT_ID);
            SerializeValue(15, obj.OPER_NAM);
            SerializeValue(16, obj.EXEC_TYP);
            SerializeValue(17, obj.EXEC_VER);
            SerializeValue(18, obj.TEST_COD);
            SerializeValue(19, obj.TST_TEMP);
            SerializeValue(20, obj.USER_TXT);
            SerializeValue(21, obj.AUX_FILE);
            SerializeValue(22, obj.PKG_TYP);
            SerializeValue(23, obj.FAMLY_ID);
            SerializeValue(24, obj.DATE_COD);
            SerializeValue(25, obj.FACIL_ID);
            SerializeValue(26, obj.FLOOR_ID);
            SerializeValue(27, obj.PROC_ID);
            SerializeValue(28, obj.OPER_FRQ);
            SerializeValue(29, obj.SPEC_NAM);
            SerializeValue(30, obj.SPEC_VER);
            SerializeValue(31, obj.FLOW_ID);
            SerializeValue(32, obj.SETUP_ID);
            SerializeValue(33, obj.DSGN_REV);
            SerializeValue(34, obj.ENG_ID);
            SerializeValue(35, obj.ROM_COD);
            SerializeValue(36, obj.SERL_NUM);
            SerializeValue(37, obj.SUPR_NAM);
        }

        public override void SetObjectData(MIR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.SETUP_T =  DeserializeValue<DateTime>(0);
            obj.START_T =  DeserializeValue<DateTime>(1);
            obj.STAT_NUM = DeserializeValue<byte>(2);
            if (CurrentInfo.IsValueSet(3)) obj.MODE_COD = DeserializeValue<char>(3);
            if (CurrentInfo.IsValueSet(4)) obj.RTST_COD = DeserializeValue<char>(4);
            if (CurrentInfo.IsValueSet(5)) obj.PROT_COD = DeserializeValue<char>(5);
            if (CurrentInfo.IsValueSet(6)) obj.BURN_TIM = DeserializeValue<ushort>(6);
            if (CurrentInfo.IsValueSet(7)) obj.CMOD_COD = DeserializeValue<char>(7);
            if (CurrentInfo.IsValueSet(8)) obj.LOT_ID   = DeserializeValue<string>(8);
            if (CurrentInfo.IsValueSet(9)) obj.PART_TYP = DeserializeValue<string>(9);
            if (CurrentInfo.IsValueSet(10)) obj.NODE_NAM = DeserializeValue<string>(10);
            if (CurrentInfo.IsValueSet(11)) obj.TSTR_TYP = DeserializeValue<string>(11);
            if (CurrentInfo.IsValueSet(12)) obj.JOB_NAM  = DeserializeValue<string>(12);
            if (CurrentInfo.IsValueSet(13)) obj.JOB_REV  = DeserializeValue<string>(13);
            if (CurrentInfo.IsValueSet(14)) obj.SBLOT_ID = DeserializeValue<string>(14);
            if (CurrentInfo.IsValueSet(15)) obj.OPER_NAM = DeserializeValue<string>(15);
            if (CurrentInfo.IsValueSet(16)) obj.EXEC_TYP = DeserializeValue<string>(16);
            if (CurrentInfo.IsValueSet(17)) obj.EXEC_VER = DeserializeValue<string>(17);
            if (CurrentInfo.IsValueSet(18)) obj.TEST_COD = DeserializeValue<string>(18);
            if (CurrentInfo.IsValueSet(19)) obj.TST_TEMP = DeserializeValue<string>(19);
            if (CurrentInfo.IsValueSet(20)) obj.USER_TXT = DeserializeValue<string>(20);
            if (CurrentInfo.IsValueSet(21)) obj.AUX_FILE = DeserializeValue<string>(21);
            if (CurrentInfo.IsValueSet(22)) obj.PKG_TYP  = DeserializeValue<string>(22);
            if (CurrentInfo.IsValueSet(23)) obj.FAMLY_ID = DeserializeValue<string>(23);
            if (CurrentInfo.IsValueSet(24)) obj.DATE_COD = DeserializeValue<string>(24);
            if (CurrentInfo.IsValueSet(25)) obj.FACIL_ID = DeserializeValue<string>(25);
            if (CurrentInfo.IsValueSet(26)) obj.FLOOR_ID = DeserializeValue<string>(26);
            if (CurrentInfo.IsValueSet(27)) obj.PROC_ID  = DeserializeValue<string>(27);
            if (CurrentInfo.IsValueSet(28)) obj.OPER_FRQ = DeserializeValue<string>(28);
            if (CurrentInfo.IsValueSet(29)) obj.SPEC_NAM = DeserializeValue<string>(29);
            if (CurrentInfo.IsValueSet(30)) obj.SPEC_VER = DeserializeValue<string>(30);
            if (CurrentInfo.IsValueSet(31)) obj.FLOW_ID  = DeserializeValue<string>(31);
            if (CurrentInfo.IsValueSet(32)) obj.SETUP_ID = DeserializeValue<string>(32);
            if (CurrentInfo.IsValueSet(33)) obj.DSGN_REV = DeserializeValue<string>(33);
            if (CurrentInfo.IsValueSet(34)) obj.ENG_ID   = DeserializeValue<string>(34);
            if (CurrentInfo.IsValueSet(35)) obj.ROM_COD  = DeserializeValue<string>(35);
            if (CurrentInfo.IsValueSet(36)) obj.SERL_NUM = DeserializeValue<string>(36);
            if (CurrentInfo.IsValueSet(37)) obj.SUPR_NAM = DeserializeValue<string>(37);
        }
    }
}
