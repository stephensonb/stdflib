namespace STDFLib
{
    /// <summary>
    /// Pin List Record Serialization Surrogate
    /// </summary>
    public class PLRSurrogate : Surrogate<PLR>
    {
        public override void GetObjectData(PLR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.GRP_INDX);
            SerializeValue(1, obj.GRP_MODE);
            SerializeValue(2, obj.GRP_RADX);
            SerializeValue(3, obj.PGM_CHAR);
            SerializeValue(4, obj.RTN_CHAR);
            SerializeValue(5, obj.PGM_CHAL);
            SerializeValue(6, obj.RTN_CHAL);
        }

        public override void SetObjectData(PLR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.GRP_INDX = DeserializeValue<ushort[]>(0);
            obj.GRP_MODE = DeserializeValue<ushort[]>(1);
            obj.GRP_RADX = DeserializeValue<byte[]>(2);
            obj.PGM_CHAR = DeserializeValue<string[]>(3);
            obj.RTN_CHAR = DeserializeValue<string[]>(4);
            obj.PGM_CHAL = DeserializeValue<string[]>(5);
            obj.RTN_CHAL = DeserializeValue<string[]>(6);
        }
    }
}
