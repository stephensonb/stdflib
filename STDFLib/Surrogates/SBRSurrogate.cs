namespace STDFLib
{
    /// <summary>
    /// Soft Bin Record Serialization Surrogate
    /// </summary>
    public class SBRSurrogate : Surrogate<SBR>
    {
        public override void GetObjectData(SBR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.HEAD_NUM);
            SerializeValue(1, obj.SITE_NUM);
            SerializeValue(2, obj.SBIN_NUM);
            SerializeValue(3, obj.SBIN_CNT);
            SerializeValue(4, obj.SBIN_PF);
            SerializeValue(5, obj.SBIN_NAM);
        }

        public override void SetObjectData(SBR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.HEAD_NUM = DeserializeValue<byte>(0);
            obj.SITE_NUM = DeserializeValue<byte>(1);
            obj.SBIN_NUM = DeserializeValue<ushort>(2);
            obj.SBIN_CNT = DeserializeValue<uint>(3);
            obj.SBIN_PF = DeserializeValue<char>(4);
            obj.SBIN_NAM = DeserializeValue<string>(5);
        }
    }
}
