namespace STDFLib
{
    /// <summary>
    /// Test Synopsis Record Serialization Surrogate
    /// </summary>
    public class TSRSurrogate : Surrogate<TSR>
    {
        protected override void SetOptionalFlags(TSR obj)
        {
            if (obj.OPT_FLAG == 0)
            {
                obj.OPT_FLAG |= (byte)(IsMissingValue(11, obj.TEST_TIM) ? (byte)TSROptionalData.TEST_TIM_Invalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(12, obj.TEST_MIN) ? (byte)TSROptionalData.TEST_MIN_Invalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(13, obj.TEST_MAX) ? (byte)TSROptionalData.TEST_MAX_Invalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(14, obj.TST_SUMS) ? (byte)TSROptionalData.TST_SUMS_Invalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(15, obj.TST_SQRS) ? (byte)TSROptionalData.TST_SQRS_Invalid : 0);
            }
        }

        public override void GetObjectData(TSR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.HEAD_NUM);
            SerializeValue(1, obj.SITE_NUM);
            SerializeValue(2, obj.TEST_TYP);
            SerializeValue(3, obj.TEST_NUM);
            SerializeValue(4, obj.EXEC_CNT);
            SerializeValue(5, obj.FAIL_CNT);
            SerializeValue(6, obj.ALRM_CNT);
            SerializeValue(7, obj.TEST_NAM);
            SerializeValue(8, obj.SEQ_NAM);
            SerializeValue(9, obj.TEST_LBL);
            SerializeValue(10, obj.OPT_FLAG);
            if ((obj.OPT_FLAG & (byte)TSROptionalData.TEST_TIM_Invalid) == 0) SerializeValue(11, obj.TEST_TIM);
            if ((obj.OPT_FLAG & (byte)TSROptionalData.TEST_MIN_Invalid) == 0) SerializeValue(12, obj.TEST_MIN);
            if ((obj.OPT_FLAG & (byte)TSROptionalData.TEST_MAX_Invalid) == 0) SerializeValue(13, obj.TEST_MAX);
            if ((obj.OPT_FLAG & (byte)TSROptionalData.TST_SUMS_Invalid) == 0) SerializeValue(14, obj.TST_SUMS);
            if ((obj.OPT_FLAG & (byte)TSROptionalData.TST_SQRS_Invalid) == 0) SerializeValue(15, obj.TST_SQRS);
        }

        public override void SetObjectData(TSR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.HEAD_NUM = DeserializeValue<byte>(0);
            obj.SITE_NUM = DeserializeValue<byte>(1);
            obj.TEST_TYP = DeserializeValue<char>(2);
            obj.TEST_NUM = DeserializeValue<uint>(3);
            obj.EXEC_CNT = DeserializeValue<uint>(4);
            obj.FAIL_CNT = DeserializeValue<uint>(5);
            obj.ALRM_CNT = DeserializeValue<uint>(6);
            obj.TEST_NAM = DeserializeValue<string>(7);
            obj.SEQ_NAM = DeserializeValue<string>(8);
            obj.TEST_LBL = DeserializeValue<string>(9);
            obj.OPT_FLAG = DeserializeValue<byte>(10);
            obj.TEST_TIM = DeserializeValue<float?>(11);
            obj.TEST_MIN = DeserializeValue<float?>(12);
            obj.TEST_MAX = DeserializeValue<float?>(13);
            obj.TST_SUMS = DeserializeValue<float?>(14);
            obj.TST_SQRS = DeserializeValue<float?>(15);
        }
    }
}
