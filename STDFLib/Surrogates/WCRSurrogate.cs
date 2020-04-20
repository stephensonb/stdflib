namespace STDFLib
{
    /// <summary>
    /// Wafer Configuration Record Serialization Surrogate
    /// </summary>
    public class WCRSurrogate : Surrogate<WCR>
    {
        public override void GetObjectData(WCR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.WAFR_SIZ);
            SerializeValue(1, obj.DIE_HT);
            SerializeValue(2, obj.DIE_WID);
            SerializeValue(3, obj.WF_UNITS);
            SerializeValue(4, obj.WF_FLAT);
            SerializeValue(5, obj.CENTER_X);
            SerializeValue(6, obj.CENTER_Y);
            SerializeValue(7, obj.POS_X);
            SerializeValue(8, obj.POS_Y);
        }

        public override void SetObjectData(WCR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.WAFR_SIZ = DeserializeValue<float?>(0);
            obj.DIE_HT = DeserializeValue<float?>(1);
            obj.DIE_WID = DeserializeValue<float?>(2);
            obj.WF_UNITS = DeserializeValue<byte?>(3);
            obj.WF_FLAT = DeserializeValue<char>(4);
            obj.CENTER_X = DeserializeValue<short>(5);
            obj.CENTER_Y = DeserializeValue<short>(6);
            obj.POS_X = DeserializeValue<char>(7);
            obj.POS_Y = DeserializeValue<char>(8);
        }
    }
}
