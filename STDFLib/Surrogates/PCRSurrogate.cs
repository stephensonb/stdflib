namespace STDFLib
{
    /// <summary>
    /// Part Count Record Serialization Surrogate
    /// </summary>
    public class PCRSurrogate : Surrogate<PCR>
    {
        public override void GetObjectData(PCR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.HEAD_NUM);
            SerializeValue(1, obj.SITE_NUM);
            SerializeValue(2, obj.PART_CNT);
            SerializeValue(3, obj.RTST_CNT);
            SerializeValue(4, obj.ABRT_CNT);
            SerializeValue(5, obj.GOOD_CNT);
            SerializeValue(6, obj.FUNC_CNT);
        }

        public override void SetObjectData(PCR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.HEAD_NUM = DeserializeValue<byte>(0);
            obj.SITE_NUM = DeserializeValue<byte>(1);
            obj.PART_CNT = DeserializeValue<uint>(2);
            obj.RTST_CNT = DeserializeValue<uint>(3);
            obj.ABRT_CNT = DeserializeValue<uint>(4);
            obj.GOOD_CNT = DeserializeValue<uint>(5);
            obj.FUNC_CNT = DeserializeValue<uint>(6);
        }
    }
}
