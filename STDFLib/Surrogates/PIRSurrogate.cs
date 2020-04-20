namespace STDFLib
{
    /// <summary>
    /// Part Information Record Serialization Surrogate
    /// </summary>
    public class PIRSurrogate : Surrogate<PIR>
    {
        public override void GetObjectData(PIR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.HEAD_NUM);
            SerializeValue(1, obj.SITE_NUM);
        }

        public override void SetObjectData(PIR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.HEAD_NUM = DeserializeValue<byte>(0);
            obj.SITE_NUM = DeserializeValue<byte>(0);
        }
    }
}
