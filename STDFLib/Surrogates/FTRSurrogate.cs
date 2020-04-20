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
            CurrentInfo.GetEntry(5).IsMissingValue = ((obj.OPT_FLAG & (byte)FTROptionalData.CycleCountDataIsInvalid) > 0);
            CurrentInfo.GetEntry(6).IsMissingValue = ((obj.OPT_FLAG & (byte)FTROptionalData.RelativeVectorAddressIsInvalid) > 0);
            CurrentInfo.GetEntry(7).IsMissingValue = ((obj.OPT_FLAG & (byte)FTROptionalData.RepeatCountOfVectorIsInvalid) > 0);
            CurrentInfo.GetEntry(8).IsMissingValue = ((obj.OPT_FLAG & (byte)FTROptionalData.NumberOfFailsIsInvalid) > 0);
            CurrentInfo.GetEntry(9).IsMissingValue = ((obj.OPT_FLAG & (byte)FTROptionalData.XYFailAddressIsInvalid) > 0);
            CurrentInfo.GetEntry(10).IsMissingValue = ((obj.OPT_FLAG & (byte)FTROptionalData.XYFailAddressIsInvalid) > 0);
            CurrentInfo.GetEntry(11).IsMissingValue = ((obj.OPT_FLAG & (byte)FTROptionalData.VectorOffsetDataIsInvalid) > 0);
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
            if (CurrentInfo.IsValueSet(5)) obj.CYCL_CNT = DeserializeValue<uint?>(5);
            if (CurrentInfo.IsValueSet(6)) obj.REL_VADR = DeserializeValue<uint?>(6);
            if (CurrentInfo.IsValueSet(7)) obj.REPT_CNT = DeserializeValue<uint?>(7);
            if (CurrentInfo.IsValueSet(8)) obj.NUM_FAIL = DeserializeValue<uint?>(8);
            if (CurrentInfo.IsValueSet(9)) obj.XFAIL_AD = DeserializeValue<int?>(9);
            if (CurrentInfo.IsValueSet(10)) obj.YFAIL_AD = DeserializeValue<int?>(10);
            if (CurrentInfo.IsValueSet(11)) obj.VECT_OFF = DeserializeValue<short?>(11);
            if (CurrentInfo.IsValueSet(12)) obj.RTN_ICNT = DeserializeValue<ushort?>(12);
            if (CurrentInfo.IsValueSet(13)) obj.PGM_ICNT = DeserializeValue<ushort?>(13);
            if (CurrentInfo.IsValueSet(14)) obj.RTN_INDX = DeserializeValue<ushort[]>(14);
            if (CurrentInfo.IsValueSet(15)) obj.RTN_STAT = DeserializeValue<byte[]>(15);
            if (CurrentInfo.IsValueSet(16)) obj.PGM_INDX = DeserializeValue<ushort[]>(16);
            if (CurrentInfo.IsValueSet(17)) obj.PGM_STAT = DeserializeValue<byte[]>(17);
            if (CurrentInfo.IsValueSet(18)) obj.FAIL_PIN = DeserializeValue<BitArray>(18);
            if (CurrentInfo.IsValueSet(19)) obj.VECT_NAM = DeserializeValue<string>(19);
            if (CurrentInfo.IsValueSet(20)) obj.TIME_SET = DeserializeValue<string>(20);
            if (CurrentInfo.IsValueSet(21)) obj.OP_CODE = DeserializeValue<string>(21);
            if (CurrentInfo.IsValueSet(22)) obj.TEST_TXT = DeserializeValue<string>(22);
            if (CurrentInfo.IsValueSet(23)) obj.ALARM_ID = DeserializeValue<string>(23);
            if (CurrentInfo.IsValueSet(24)) obj.PROG_TXT = DeserializeValue<string>(24);
            if (CurrentInfo.IsValueSet(25)) obj.RSLT_TXT = DeserializeValue<string>(25);
            // Set original values for fields that could be changed due to using a default value
            if (CurrentInfo.IsValueSet(26))
            {
                obj.PATG_NUM = DeserializeValue<byte?>(26);
                obj.ORIG_PATG_NUM = info.GetValue<byte?>(26);
            }
            if (CurrentInfo.IsValueSet(27))
            {
                obj.SPIN_MAP = DeserializeValue<ByteArray>(27);
                obj.ORIG_SPIN_MAP = info.GetValue<ByteArray>(27);
            }

            SetOptionalFlags(obj);

            AddDefaults();
        }
    }
}