namespace STDFLib
{
    /// <summary>
    /// Hard Bin Record Serialization Surrogate
    /// </summary>
    public class HBRSurrogate : Surrogate<HBR>
    {
        public override void GetObjectData(HBR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.HEAD_NUM);
            SerializeValue(1, obj.SITE_NUM);
            SerializeValue(2, obj.HBIN_NUM);
            SerializeValue(3, obj.HBIN_CNT);
            SerializeValue(4, obj.HBIN_PF);
            SerializeValue(5, obj.HBIN_NAM);
        }

        public override void SetObjectData(HBR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.HEAD_NUM = DeserializeValue<byte>(0);
            obj.SITE_NUM = DeserializeValue<byte>(1);
            obj.HBIN_NUM = DeserializeValue<ushort>(2);
            obj.HBIN_CNT = DeserializeValue<uint>(3);
            obj.HBIN_PF = DeserializeValue<char>(4);
            obj.HBIN_NAM = DeserializeValue<string>(5);
        }
    }
}
