namespace STDFLib
{
    /// <summary>
    /// File Attributes Record Serialization Surrogate
    /// </summary>
    public class FARSurrogate : Surrogate<FAR>
    {
        public override void GetObjectData(FAR obj, SerializationInfo info)
        {
            base.GetObjectData(obj, info);

            SerializeValue(0, obj.CPU_TYPE);
            SerializeValue(1, obj.STDF_VER);
        }

        public override void SetObjectData(FAR obj, SerializationInfo info)
        {
            base.SetObjectData(obj, info);

            obj.CPU_TYPE = DeserializeValue<byte>(0);
            obj.STDF_VER = DeserializeValue<byte>(1);
        }
    }
}
