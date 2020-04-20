namespace STDFLib
{
    /// <summary>
    /// Parametric Test Record Serialization Surrogate
    /// </summary>
    public class PTRSurrogate : Surrogate<PTR>
    {
        protected override ulong GetHashKey()
        {
            return CurrentObject.TEST_NUM;
        }

        protected override void SetOptionalFlags(PTR obj)
        {
            if (obj.TEST_FLG == 0)
            {
                obj.TEST_FLG |= (byte)(IsMissingValue(5, obj.RESULT) ? TestResultFlags.InvalidResult : 0);
            }

            // If the optional data flag is not set, then set it based on the data in the relevant fields
            if (obj.OPT_FLAG == 0)
            {
                obj.OPT_FLAG |= (byte)(IsMissingValue(9, obj.RES_SCAL) ? PTROptionalData.RES_SCAL_Invalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(10, obj.LLM_SCAL) ? PTROptionalData.LO_LIMIT_LLM_SCAL_Invalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(11, obj.HLM_SCAL) ? PTROptionalData.HI_LIMIT_HLM_SCAL_Invalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(12, obj.LO_LIMIT) ? PTROptionalData.NoLoLimitThisTest : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(13, obj.HI_LIMIT) ? PTROptionalData.NoHiLimitThisTest : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(14, obj.LO_SPEC) ? PTROptionalData.NoLoLimitSpec : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(15, obj.HI_SPEC) ? PTROptionalData.NoHiLimitSpec : 0);
            }
        }

        public override void GetObjectData(PTR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            GetDefaults();

            SetOptionalFlags(obj);

            SerializeValue(0, obj.TEST_NUM);
            SerializeValue(1, obj.HEAD_NUM);
            SerializeValue(2, obj.SITE_NUM);
            SerializeValue(3, obj.TEST_FLG);
            SerializeValue(4, obj.PARM_FLG);
            SerializeValue(5, obj.RESULT);
            SerializeValue(6, obj.TEST_TXT);
            SerializeValue(7, obj.ALARM_ID);
            SerializeValue(8, obj.OPT_FLAG);
            SerializeValue(9, obj.RES_SCAL, obj.ORIG_RES_SCAL);
            SerializeValue(10, obj.LLM_SCAL, obj.ORIG_LLM_SCAL);
            SerializeValue(11, obj.HLM_SCAL, obj.ORIG_HLM_SCAL);
            SerializeValue(12, obj.LO_LIMIT, obj.ORIG_LO_LIMIT);
            SerializeValue(13, obj.HI_LIMIT, obj.ORIG_HI_LIMIT);
            SerializeValue(14, obj.UNITS, obj.ORIG_UNITS);
            SerializeValue(15, obj.C_RESFMT, obj.ORIG_C_RESFMT);
            SerializeValue(16, obj.C_LLMFMT, obj.ORIG_C_LLMFMT);
            SerializeValue(17, obj.C_HLMFMT, obj.ORIG_C_HLMFMT);
            SerializeValue(18, obj.LO_SPEC, obj.ORIG_LO_SPEC);
            SerializeValue(19, obj.HI_SPEC, obj.ORIG_HI_SPEC);

            // Set missing value flag on OPT_FLAG.  We can truncate this from the record if all fields after the OPT_FLAG field
            // are missing also.  If not, it will be written anyway.
            CurrentInfo.GetEntry(8).IsMissingValue = true;

            AddDefaults();
        }

        public override void SetObjectData(PTR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.TEST_NUM = DeserializeValue<uint>(0);
            obj.HEAD_NUM = DeserializeValue<byte>(1);
            obj.SITE_NUM = DeserializeValue<byte>(2);

            GetDefaults();

            obj.TEST_FLG = DeserializeValue<byte>(3);
            obj.PARM_FLG = DeserializeValue<byte>(4);
            obj.RESULT = DeserializeValue<float>(5);
            obj.TEST_TXT = DeserializeValue<string>(6);
            obj.ALARM_ID = DeserializeValue<string>(7);
            obj.OPT_FLAG = DeserializeValue<byte>(8);
            obj.RES_SCAL = DeserializeValue<sbyte?>(9);
            obj.ORIG_RES_SCAL = info.GetValue<sbyte?>(9);
            obj.LLM_SCAL = DeserializeValue<sbyte?>(10);
            obj.ORIG_LLM_SCAL = info.GetValue<sbyte?>(10);
            obj.HLM_SCAL = DeserializeValue<sbyte?>(11);
            obj.ORIG_HLM_SCAL = info.GetValue<sbyte?>(11);
            obj.LO_LIMIT = DeserializeValue<float?>(12);
            obj.ORIG_LO_LIMIT = info.GetValue<float?>(12);
            obj.HI_LIMIT = DeserializeValue<float?>(13);
            obj.ORIG_HI_LIMIT = info.GetValue<float?>(13);
            obj.UNITS = DeserializeValue<string>(14);
            obj.ORIG_UNITS = info.GetValue<string>(14);
            obj.C_RESFMT = DeserializeValue<string>(15);
            obj.ORIG_C_RESFMT = info.GetValue<string>(15);
            obj.C_LLMFMT = DeserializeValue<string>(16);
            obj.ORIG_C_LLMFMT = info.GetValue<string>(16);
            obj.C_HLMFMT = DeserializeValue<string>(17);
            obj.ORIG_C_HLMFMT = info.GetValue<string>(17);
            obj.LO_SPEC = DeserializeValue<float?>(18);
            obj.ORIG_LO_SPEC = info.GetValue<float?>(18);
            obj.HI_SPEC = DeserializeValue<float?>(19);
            obj.ORIG_HI_SPEC = info.GetValue<float?>(19);

            SetOptionalFlags(obj);

            AddDefaults();
        }
    }
}
