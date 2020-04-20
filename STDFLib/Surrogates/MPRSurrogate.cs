namespace STDFLib
{
    /// <summary>
    /// Multiresult Parametric Record Serialization Surrogate
    /// </summary>
    public class MPRSurrogate : Surrogate<MPR>
    {
        protected override ulong GetHashKey()
        {
            return CurrentObject.TEST_NUM;
        }

        protected override void SetOptionalFlags(MPR obj)
        {
            // If the optional data flag is not set, then set it based on the data in the relevant fields
            if (obj.OPT_FLAG == 0)
            {
                obj.OPT_FLAG |= (byte)(IsMissingValue(12, obj.RES_SCAL) ? MPROptionalData.RES_SCAL_Invalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(13, obj.LLM_SCAL) ? MPROptionalData.LO_LIMIT_LLM_SCAL_Invalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(14, obj.HLM_SCAL) ? MPROptionalData.HI_LIMIT_HLM_SCAL_Invalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(15, obj.START_IN) ? MPROptionalData.START_IN_INCR_IN_Invalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(16, obj.LO_LIMIT) ? MPROptionalData.NoLoLimitThisTest : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(17, obj.HI_LIMIT) ? MPROptionalData.NoHiLimitThisTest : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(19, obj.LO_SPEC) ? MPROptionalData.NoLoLimitSpec : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(20, obj.HI_SPEC) ? MPROptionalData.NoHiLimitSpec : 0);
            }
        }

        public override void GetObjectData(MPR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            GetDefaults();

            SetOptionalFlags(obj);

            // set array counts
            obj.RTN_ICNT = (ushort)obj.RTN_INDX.Length;
            obj.RSLT_CNT = (ushort)obj.RTN_RSLT.Length;

            info.SetValue(0, obj.TEST_NUM);
            info.SetValue(1, obj.HEAD_NUM);
            info.SetValue(2, obj.SITE_NUM);
            info.SetValue(3, obj.TEST_FLG);
            info.SetValue(4, obj.PARM_FLG);
            info.SetValue(5, obj.RTN_ICNT);
            info.SetValue(6, obj.RSLT_CNT);
            if (obj.RTN_ICNT > 0) SerializeValue(7, obj.RTN_STAT);
            if (obj.RTN_ICNT > 0) SerializeValue(8, obj.RTN_RSLT);
            info.SetValue(9, obj.TEST_TXT);
            info.SetValue(10, obj.ALARM_ID);
            info.SetValue(11, obj.OPT_FLAG);

            SerializeValue(12, obj.RES_SCAL, obj.ORIG_RES_SCAL);
            SerializeValue(13, obj.LLM_SCAL, obj.ORIG_LLM_SCAL);
            SerializeValue(14, obj.HLM_SCAL, obj.ORIG_HLM_SCAL);
            SerializeValue(15, obj.LO_LIMIT, obj.ORIG_LO_LIMIT);
            SerializeValue(16, obj.HI_LIMIT, obj.ORIG_HI_LIMIT);
            SerializeValue(17, obj.START_IN, obj.ORIG_START_IN);
            SerializeValue(18, obj.INCR_IN, obj.ORIG_INCR_IN);
            SerializeValue(19, obj.RTN_INDX, obj.ORIG_RTN_INDX);
            SerializeValue(20, obj.UNITS, obj.ORIG_UNITS);
            SerializeValue(21, obj.UNITS_IN, obj.ORIG_UNITS_IN);
            SerializeValue(22, obj.C_RESFMT, obj.ORIG_C_RESFMT);
            SerializeValue(23, obj.C_LLMFMT, obj.ORIG_C_LLMFMT);
            SerializeValue(24, obj.C_HLMFMT, obj.ORIG_C_HLMFMT);
            SerializeValue(25, obj.LO_SPEC, obj.ORIG_LO_SPEC);
            SerializeValue(26, obj.HI_SPEC, obj.ORIG_HI_SPEC);

            CurrentInfo.GetEntry(12).IsMissingValue = ((obj.OPT_FLAG & (byte)MPROptionalData.RES_SCAL_Invalid) > 0);
            CurrentInfo.GetEntry(13).IsMissingValue = ((obj.OPT_FLAG & (byte)MPROptionalData.LO_LIMIT_LLM_SCAL_Invalid) > 0);
            CurrentInfo.GetEntry(14).IsMissingValue = ((obj.OPT_FLAG & (byte)MPROptionalData.HI_LIMIT_HLM_SCAL_Invalid) > 0);
            CurrentInfo.GetEntry(15).IsMissingValue = ((obj.OPT_FLAG & (byte)MPROptionalData.NoLoLimitThisTest) > 0);
            CurrentInfo.GetEntry(16).IsMissingValue = ((obj.OPT_FLAG & (byte)MPROptionalData.NoHiLimitThisTest) > 0);
            CurrentInfo.GetEntry(25).IsMissingValue = ((obj.OPT_FLAG & (byte)MPROptionalData.NoLoLimitSpec) > 0);
            CurrentInfo.GetEntry(26).IsMissingValue = ((obj.OPT_FLAG & (byte)MPROptionalData.NoHiLimitSpec) > 0);

            if ((obj.OPT_FLAG & (byte)MPROptionalData.START_IN_INCR_IN_Invalid) == 0)
            {
                CurrentInfo.GetEntry(17).IsMissingValue = false;
                CurrentInfo.GetEntry(18).IsMissingValue = false;
            }

            AddDefaults();
        }

        public override void SetObjectData(MPR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.TEST_NUM = DeserializeValue<uint>(0);
            obj.HEAD_NUM = DeserializeValue<byte>(1);
            obj.SITE_NUM = DeserializeValue<byte>(2);

            GetDefaults();

            obj.TEST_FLG = DeserializeValue<byte>(3);
            obj.PARM_FLG = DeserializeValue<byte>(4);
            obj.RTN_ICNT = DeserializeValue<ushort>(5);
            obj.RSLT_CNT = DeserializeValue<ushort>(6);
            obj.RTN_STAT = DeserializeValue<byte[]>(7);
            obj.RTN_RSLT = DeserializeValue<float[]>(8);
            obj.TEST_TXT = DeserializeValue<string>(9);
            obj.ALARM_ID = DeserializeValue<string>(10);
            obj.OPT_FLAG = DeserializeValue<byte>(11);
            if (CurrentInfo.IsValueSet(12))
            {
                obj.RES_SCAL = DeserializeValue<sbyte?>(12);
                obj.ORIG_RES_SCAL = info.GetValue<sbyte?>(12);
            }
            if (CurrentInfo.IsValueSet(13))
            {
                obj.LLM_SCAL = DeserializeValue<sbyte?>(13);
                obj.ORIG_LLM_SCAL = info.GetValue<sbyte?>(13);
            }
            if (CurrentInfo.IsValueSet(14))
            {
                obj.HLM_SCAL = DeserializeValue<sbyte?>(14);
                obj.ORIG_HLM_SCAL = info.GetValue<sbyte?>(14);
            }
            if (CurrentInfo.IsValueSet(15))
            {
                obj.LO_LIMIT = DeserializeValue<float?>(15);
                obj.ORIG_LO_LIMIT = info.GetValue<float?>(15);
            }
            if (CurrentInfo.IsValueSet(16))
            {
                obj.HI_LIMIT = DeserializeValue<float?>(16);
                obj.ORIG_HI_LIMIT = info.GetValue<float?>(16);
            }
            if (CurrentInfo.IsValueSet(17))
            {
                obj.START_IN = DeserializeValue<float?>(17);
                obj.ORIG_START_IN = info.GetValue<float?>(17);
            }
            if (CurrentInfo.IsValueSet(18))
            {
                obj.INCR_IN = DeserializeValue<float?>(18);
                obj.ORIG_INCR_IN = info.GetValue<float?>(18);
            }
            if (CurrentInfo.IsValueSet(19))
            {
                obj.RTN_INDX = DeserializeValue<ushort[]>(19);
                obj.ORIG_RTN_INDX = info.GetValue<ushort[]>(19);
            }
            if (CurrentInfo.IsValueSet(20))
            {
                obj.UNITS = DeserializeValue<string>(20);
                obj.ORIG_UNITS = info.GetValue<string>(20);
            }
            if (CurrentInfo.IsValueSet(21))
            {
                obj.UNITS_IN = DeserializeValue<string>(21);
                obj.ORIG_UNITS_IN = info.GetValue<string>(21);
            }
            if (CurrentInfo.IsValueSet(22))
            {
                obj.C_RESFMT = DeserializeValue<string>(22);
                obj.ORIG_C_RESFMT = info.GetValue<string>(22);
            }
            if (CurrentInfo.IsValueSet(23))
            {
                obj.C_LLMFMT = DeserializeValue<string>(23);
                obj.ORIG_C_LLMFMT = info.GetValue<string>(23);
            }
            if (CurrentInfo.IsValueSet(24))
            {
                obj.C_HLMFMT = DeserializeValue<string>(24);
                obj.ORIG_C_HLMFMT = info.GetValue<string>(24);
            }
            if (CurrentInfo.IsValueSet(25))
            {
                obj.LO_SPEC = DeserializeValue<float?>(25);
                obj.ORIG_LO_SPEC = info.GetValue<float?>(25);
            }
            if (CurrentInfo.IsValueSet(26))
            {
                obj.HI_SPEC = DeserializeValue<float?>(26);
                obj.ORIG_HI_SPEC = info.GetValue<float?>(26);
            }

            SetOptionalFlags(obj);

            // set the original value fields for later serialization logic

            AddDefaults();
        }
    }
}
