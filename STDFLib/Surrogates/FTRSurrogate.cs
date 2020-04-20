namespace STDFLib
{
    /// <summary>
    /// Functional Test Record Serialization Surrogate
    /// </summary>
    public class FTRSurrogate : Surrogate<FTR>
    {
        protected override ulong GetHashKey()
        {
            return CurrentObject.TEST_NUM;
        }

        protected override void SetOptionalFlags(FTR obj)
        {
            // If the optional data flag is not set, then set it based on the data in the relevant fields
            if (obj.OPT_FLAG == 0)
            {
                obj.OPT_FLAG |= (byte)(IsMissingValue(5, obj.CYCL_CNT) ? FTROptionalData.CycleCountDataIsInvalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(6, obj.REL_VADR) ? FTROptionalData.RelativeVectorAddressIsInvalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(7, obj.REPT_CNT) ? FTROptionalData.RepeatCountOfVectorIsInvalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(8, obj.NUM_FAIL) ? FTROptionalData.NumberOfFailsIsInvalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(9, obj.XFAIL_AD) ? FTROptionalData.XYFailAddressIsInvalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(10, obj.YFAIL_AD) ? FTROptionalData.XYFailAddressIsInvalid : 0);
                obj.OPT_FLAG |= (byte)(IsMissingValue(11, obj.VECT_OFF) ? FTROptionalData.VectorOffsetDataIsInvalid : 0);
            }
        }

        // Populate serialization info with data from object for serialization
        public override void GetObjectData(FTR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            GetDefaults();

            SetOptionalFlags(obj);

            // set array counts
            obj.RTN_ICNT = (ushort)(obj.RTN_INDX?.Length ?? 0);
            obj.PGM_ICNT = (ushort)(obj.PGM_INDX?.Length ?? 0);
            SerializeValue(0, obj.TEST_NUM);
            SerializeValue(1, obj.HEAD_NUM);
            SerializeValue(2, obj.SITE_NUM);
            SerializeValue(3, obj.TEST_FLG);
            SerializeValue(4, obj.OPT_FLAG);
            SerializeValue(5, obj.CYCL_CNT);
            SerializeValue(6, obj.REL_VADR);
            SerializeValue(7, obj.REPT_CNT);
            SerializeValue(8, obj.NUM_FAIL);
            SerializeValue(9, obj.XFAIL_AD);
            SerializeValue(10, obj.YFAIL_AD);
            SerializeValue(11, obj.VECT_OFF);
            SerializeValue(12, obj.RTN_ICNT);
            SerializeValue(13, obj.PGM_ICNT);
            if (obj.RTN_ICNT > 0) SerializeValue(14, obj.RTN_INDX);
            if (obj.RTN_ICNT > 0) SerializeValue(15, obj.RTN_STAT);
            if (obj.PGM_ICNT > 0) SerializeValue(16, obj.PGM_INDX);
            if (obj.PGM_ICNT > 0) SerializeValue(17, obj.PGM_STAT);
            SerializeValue(18, obj.FAIL_PIN);
            SerializeValue(19, obj.VECT_NAM);
            SerializeValue(20, obj.TIME_SET);
            SerializeValue(21, obj.OP_CODE);
            SerializeValue(22, obj.TEST_TXT);
            SerializeValue(23, obj.ALARM_ID);
            SerializeValue(24, obj.PROG_TXT);
            SerializeValue(25, obj.RSLT_TXT);
            SerializeValue(26, obj.PATG_NUM, obj.ORIG_PATG_NUM);
            SerializeValue(27, obj.SPIN_MAP, obj.ORIG_SPIN_MAP);

            AddDefaults();
        }

        public override void SetObjectData(FTR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            // Set these fields first, they are used to get the correct default values (if they exist) for the current record
            // being deserialized
            obj.TEST_NUM = DeserializeValue<uint>(0);
            obj.HEAD_NUM = DeserializeValue<byte>(1);
            obj.SITE_NUM = DeserializeValue<byte>(2);

            // get the defaults for this record
            GetDefaults();

            obj.TEST_FLG = DeserializeValue<byte>(3);
            obj.OPT_FLAG = DeserializeValue<byte>(4);
            obj.CYCL_CNT = DeserializeValue<uint?>(5);
            obj.REL_VADR = DeserializeValue<uint?>(6);
            obj.REPT_CNT = DeserializeValue<uint?>(7);
            obj.NUM_FAIL = DeserializeValue<uint?>(8);
            obj.XFAIL_AD = DeserializeValue<int?>(9);
            obj.YFAIL_AD = DeserializeValue<int?>(10);
            obj.VECT_OFF = DeserializeValue<short?>(11);
            obj.RTN_ICNT = DeserializeValue<ushort?>(12);
            obj.PGM_ICNT = DeserializeValue<ushort?>(13);
            obj.RTN_INDX = DeserializeValue<ushort[]>(14);
            obj.RTN_STAT = DeserializeValue<byte[]>(15);
            obj.PGM_INDX = DeserializeValue<ushort[]>(16);
            obj.PGM_STAT = DeserializeValue<byte[]>(17);
            obj.FAIL_PIN = DeserializeValue<BitArray>(18);
            obj.VECT_NAM = DeserializeValue<string>(19);
            obj.TIME_SET = DeserializeValue<string>(20);
            obj.OP_CODE = DeserializeValue<string>(21);
            obj.TEST_TXT = DeserializeValue<string>(22);
            obj.ALARM_ID = DeserializeValue<string>(23);
            obj.PROG_TXT = DeserializeValue<string>(24);
            obj.RSLT_TXT = DeserializeValue<string>(25);
            obj.PATG_NUM = DeserializeValue<byte?>(26);
            obj.ORIG_PATG_NUM = info.GetValue<byte?>(26);
            obj.SPIN_MAP = DeserializeValue<ByteArray>(27);
            obj.ORIG_SPIN_MAP = info.GetValue<ByteArray>(27);

            SetOptionalFlags(obj);

            AddDefaults();
        }
    }
}